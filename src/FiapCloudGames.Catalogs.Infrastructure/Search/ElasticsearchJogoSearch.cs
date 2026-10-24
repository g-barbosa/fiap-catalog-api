using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FiapCloudGames.Catalogs.Infrastructure.Search
{
    public sealed class ElasticsearchJogoSearch : IJogoSearch
    {
        private readonly ElasticsearchClient _client;
        private readonly string _index;

        public ElasticsearchJogoSearch(IConfiguration configuration)
        {
            var url = configuration["Elasticsearch:Url"]
                ?? throw new InvalidOperationException("Elasticsearch:Url não configurada.");
            _index = configuration["Elasticsearch:Index"] ?? "jogos";

            var settings = new ElasticsearchClientSettings(new Uri(url))
                .DefaultMappingFor<JogoDocument>(mapping => mapping
                    .IndexName(_index)
                    .IdProperty(document => document.Id));

            var apiKey = configuration["Elasticsearch:ApiKey"];
            if (!string.IsNullOrWhiteSpace(apiKey))
                settings = settings.Authentication(new Base64ApiKey(apiKey));

            _client = new ElasticsearchClient(settings);
        }

        public async Task IndexarAsync(JogoResponse jogo, CancellationToken cancellationToken = default)
        {
            var document = JogoDocument.FromResponse(jogo);
            var response = await _client.IndexAsync(document, _index, document.Id, cancellationToken);
            GarantirSucesso(response);
        }

        public async Task RemoverAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await _client.DeleteAsync(_index, id.ToString(), cancellationToken);
            if (response.IsValidResponse || response.Result == Result.NotFound)
                return;

            GarantirSucesso(response);
        }

        public async Task<IReadOnlyList<JogoBuscaResponse>> BuscarAsync(string termo, CancellationToken cancellationToken = default)
        {
            var response = await _client.SearchAsync<JogoDocument>(search => search
                .Indices(_index)
                .Size(20)
                .Query(query => query
                    .MultiMatch(match => match
                        .Fields(document => document.Titulo, document => document.Descricao)
                        .Query(termo)
                        .Fuzziness(new Fuzziness("AUTO"))))
                .Sort(sort => sort.Score(score => score.Order(SortOrder.Desc))), cancellationToken);

            GarantirSucesso(response);

            return response.Hits
                .Select(hit =>
                {
                    var source = hit.Source ?? new JogoDocument();
                    return new JogoBuscaResponse
                    {
                        Id = Guid.TryParse(source.Id, out var id) ? id : Guid.Empty,
                        Titulo = source.Titulo,
                        Descricao = source.Descricao,
                        Preco = (decimal)source.Preco,
                        Score = hit.Score ?? 0
                    };
                })
                .ToList();
        }

        public async Task SincronizarAsync(IEnumerable<JogoResponse> jogos, CancellationToken cancellationToken = default)
        {
            await GarantirIndiceAsync(cancellationToken);

            foreach (var jogo in jogos)
                await IndexarAsync(jogo, cancellationToken);
        }

        private async Task GarantirIndiceAsync(CancellationToken cancellationToken)
        {
            var exists = await _client.Indices.ExistsAsync(_index, cancellationToken);
            if (exists.Exists)
                return;

            var created = await _client.Indices.CreateAsync(_index, create => create
                .Mappings(mappings => mappings
                    .Properties<JogoDocument>(properties => properties
                        .Keyword(document => document.Id)
                        .Text(document => document.Titulo)
                        .Text(document => document.Descricao)
                        .FloatNumber(document => document.Preco))), cancellationToken);

            GarantirSucesso(created);
        }

        private static void GarantirSucesso(TransportResponse response)
        {
            if (response.ApiCallDetails?.HasSuccessfulStatusCode == true)
                return;

            var detalhe = response.ApiCallDetails?.DebugInformation ?? "sem detalhes";
            throw new InvalidOperationException($"Elasticsearch recusou a operação. {detalhe}");
        }
    }
}
