using FiapCloudGames.Catalogs.Domain.Avaliacoes.Entities;

namespace FiapCloudGames.Catalogs.Domain.Avaliacoes.Interfaces
{
    public interface IAvaliacaoRepository
    {
        Task AdicionarAsync(Avaliacao avaliacao);
        Task<IReadOnlyList<Avaliacao>> ObterPorJogoIdAsync(Guid jogoId);
    }
}