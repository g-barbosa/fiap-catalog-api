using FiapCloudGames.Catalogs.Application.DTOs.Jogos;

namespace FiapCloudGames.Catalogs.Application.Jogos.Interfaces
{
    public interface IJogoService
    {
        Task<IEnumerable<JogoResponse>> ObterTodosAsync();
        Task<JogoResponse?> ObterPorIdAsync(Guid id);
        Task<JogoResponse> CriarAsync(JogoRequest request);
        Task<JogoResponse?> AtualizarAsync(Guid id, JogoRequest request);
        Task<bool> DeletarAsync(Guid id);
    }
}