using FiapCloudGames.Catalogs.Application.DTOs.Avaliacoes;

namespace FiapCloudGames.Catalogs.Application.Avaliacoes.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoResponse?> CriarAsync(Guid jogoId, AvaliacaoRequest request);
        Task<IReadOnlyList<AvaliacaoResponse>?> ObterPorJogoIdAsync(Guid jogoId);
    }
}