using FiapCloudGames.Catalogs.Application.DTOs.Jogos;

namespace FiapCloudGames.Catalogs.Application.Jogos.Interfaces
{
    /// <summary>
    /// Porta de cache do caso de uso de jogos.
    /// Trabalha com <see cref="JogoResponse"/> (DTO da Application); a implementação (Redis) fica na Infrastructure.
    /// </summary>
    public interface IJogoCache
    {
        Task<IReadOnlyList<JogoResponse>?> ObterTodosAsync();
        Task DefinirTodosAsync(IEnumerable<JogoResponse> jogos);
        Task<JogoResponse?> ObterPorIdAsync(Guid id);
        Task DefinirPorIdAsync(JogoResponse jogo);
        Task InvalidarAsync(Guid? id = null);
    }
}