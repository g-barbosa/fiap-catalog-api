using FiapCloudGames.Catalogs.Application.Avaliacoes.Interfaces;
using FiapCloudGames.Catalogs.Application.DTOs.Avaliacoes;
using FiapCloudGames.Catalogs.Domain.Avaliacoes.Entities;
using FiapCloudGames.Catalogs.Domain.Avaliacoes.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;

namespace FiapCloudGames.Catalogs.Application.Avaliacoes.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IJogoRepository _jogoRepository;

        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository, IJogoRepository jogoRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _jogoRepository = jogoRepository;
        }

        public async Task<AvaliacaoResponse?> CriarAsync(Guid jogoId, AvaliacaoRequest request)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo is null)
                return null;

            var avaliacao = new Avaliacao(
                jogoId,
                request.UsuarioId,
                request.NomeUsuario,
                request.Nota,
                request.Comentario);

            await _avaliacaoRepository.AdicionarAsync(avaliacao);
            return ToResponse(avaliacao);
        }

        public async Task<IReadOnlyList<AvaliacaoResponse>?> ObterPorJogoIdAsync(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo is null)
                return null;

            var avaliacoes = await _avaliacaoRepository.ObterPorJogoIdAsync(jogoId);
            return [.. avaliacoes.Select(ToResponse)];
        }

        private static AvaliacaoResponse ToResponse(Avaliacao avaliacao) => new()
        {
            Id = avaliacao.Id,
            JogoId = avaliacao.JogoId,
            UsuarioId = avaliacao.UsuarioId,
            NomeUsuario = avaliacao.NomeUsuario,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            DataCriacao = avaliacao.DataCriacao
        };
    }
}