using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Catalogs.Infrastructure.Search
{
    /// <summary>
    /// Recria o índice a partir do SQL na subida, para a busca não nascer vazia.
    /// </summary>
    public sealed class JogoSearchSyncService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JogoSearchSyncService> _logger;

        public JogoSearchSyncService(IServiceScopeFactory scopeFactory, ILogger<JogoSearchSyncService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IJogoRepository>();
                var search = scope.ServiceProvider.GetRequiredService<IJogoSearch>();

                var jogos = (await repository.ObterTodosAsync())
                    .Select(jogo => new JogoResponse
                    {
                        Id = jogo.Id,
                        Titulo = jogo.Titulo,
                        Descricao = jogo.Descricao,
                        Preco = jogo.Preco,
                        DataCriacao = jogo.DataCriacao,
                        DataAtualizacao = jogo.DataAtualizacao
                    })
                    .ToList();

                await search.SincronizarAsync(jogos, cancellationToken);
                _logger.LogInformation("Índice de jogos sincronizado com {Quantidade} documentos.", jogos.Count);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Falha ao sincronizar o índice de jogos no Elasticsearch.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
