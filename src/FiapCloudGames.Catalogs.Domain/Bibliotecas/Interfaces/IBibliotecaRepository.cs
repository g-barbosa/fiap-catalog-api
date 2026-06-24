using FiapCloudGames.Catalogs.Domain.Bibliotecas.Entities;

namespace FiapCloudGames.Catalogs.Domain.Bibliotecas.Interfaces
{
    public interface IBibliotecaRepository
    {
        Task<IEnumerable<Biblioteca>> ObterTodosAsync();

        Task<Biblioteca?> ObterPorIdAsync(Guid id);

        Task<Biblioteca?> ObterPorUsuarioIdAsync(Guid usuarioId);

        Task AdicionarAsync(Biblioteca biblioteca);

        Task AtualizarAsync(Biblioteca biblioteca);

        Task<bool> RemoverAsync(Guid id);
    }
}
