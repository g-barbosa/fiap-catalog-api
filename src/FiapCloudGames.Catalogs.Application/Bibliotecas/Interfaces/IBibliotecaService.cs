using FiapCloudGames.Catalogs.Application.DTOs.Bibliotecas;

namespace FiapCloudGames.Catalogs.Application.Bibliotecas.Interfaces
{
    public interface IBibliotecaService
    {
        Task<IEnumerable<BibliotecaResponse>> ObterTodosAsync();
        Task<BibliotecaResponse?> ObterPorIdAsync(Guid id);
        Task<BibliotecaResponse?> ObterPorUsuarioIdAsync(Guid usuarioId);
        Task<BibliotecaResponse> CriarAsync(BibliotecaRequest request);
        Task<BibliotecaResponse?> AdicionarJogoAsync(Guid bibliotecaId, Guid jogoId);
        Task<BibliotecaResponse?> RemoverJogoAsync(Guid bibliotecaId, Guid jogoId);
        Task<bool> DeletarAsync(Guid id);
    }
}
