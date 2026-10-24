using FiapCloudGames.Catalogs.Application.DTOs.Jogos;

namespace FiapCloudGames.Catalogs.Application.Jogos.Interfaces
{
    /// <summary>
    /// Porta de busca de jogos. A implementação (Elasticsearch) fica na Infrastructure
    /// e não consulta o banco relacional.
    /// </summary>
    public interface IJogoSearch
    {
        Task IndexarAsync(JogoResponse jogo, CancellationToken cancellationToken = default);
        Task RemoverAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JogoBuscaResponse>> BuscarAsync(string termo, CancellationToken cancellationToken = default);
        Task SincronizarAsync(IEnumerable<JogoResponse> jogos, CancellationToken cancellationToken = default);
    }
}
