using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;

namespace FiapCloudGames.Catalogs.Application.Jogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<IEnumerable<JogoResponse>> ObterTodosAsync()
        {
            var jogos = await _jogoRepository.ObterTodosAsync();
            return jogos.Select(ToResponse);
        }

        public async Task<JogoResponse?> ObterPorIdAsync(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            return jogo is null ? null : ToResponse(jogo);
        }

        public async Task<JogoResponse> CriarAsync(JogoRequest request)
        {
            var jogo = new Jogo(request.Titulo, request.Descricao);
            await _jogoRepository.AdicionarAsync(jogo);
            return ToResponse(jogo);
        }

        public async Task<JogoResponse?> AtualizarAsync(Guid id, JogoRequest request)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo is null)
                return null;

            jogo.Titulo = request.Titulo;
            jogo.Descricao = request.Descricao;
            jogo.DataAtualizacao = DateTime.UtcNow;

            await _jogoRepository.AtualizarAsync(jogo);
            return ToResponse(jogo);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            return await _jogoRepository.RemoverAsync(id);
        }

        private static JogoResponse ToResponse(Jogo jogo) => new()
        {
            Id = jogo.Id,
            Titulo = jogo.Titulo,
            Descricao = jogo.Descricao,
            DataCriacao = jogo.DataCriacao,
            DataAtualizacao = jogo.DataAtualizacao
        };
    }
}