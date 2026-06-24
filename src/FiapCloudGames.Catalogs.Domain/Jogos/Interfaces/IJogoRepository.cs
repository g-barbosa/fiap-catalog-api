using FiapCloudGames.Catalogs.Domain.Jogos.Entities;

namespace FiapCloudGames.Catalogs.Domain.Jogos.Interfaces
{
    /// <summary>
    /// Define o contrato de persistência para a entidade <see cref="Jogo"/>.
    /// </summary>
    public interface IJogoRepository
    {
        /// <summary>Retorna todos os jogos cadastrados.</summary>
        Task<IEnumerable<Jogo>> ObterTodosAsync();

        /// <summary>Retorna um jogo pelo seu identificador, ou <c>null</c> se não encontrado.</summary>
        Task<Jogo?> ObterPorIdAsync(Guid id);

        /// <summary>Adiciona um novo jogo ao repositório de forma assíncrona.</summary>
        Task AdicionarAsync(Jogo jogo);

        /// <summary>Atualiza os dados de um jogo existente.</summary>
        Task AtualizarAsync(Jogo jogo);

        /// <summary>Remove um jogo pelo seu identificador. Retorna <c>false</c> se não encontrado.</summary>
        Task<bool> RemoverAsync(Guid id);
    }
}
