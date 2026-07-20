using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;

namespace FiapCloudGames.Catalogs.Application.Jogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogoCache _jogoCache;

        public JogoService(IJogoRepository jogoRepository, IJogoCache jogoCache)
        {
            _jogoRepository = jogoRepository;
            _jogoCache = jogoCache;
        }

        public async Task<IEnumerable<JogoResponse>> ObterTodosAsync()
        {
            var cached = await _jogoCache.ObterTodosAsync();
            if (cached is not null)
                return cached;

            var jogos = (await _jogoRepository.ObterTodosAsync())
                .Select(ToResponse)
                .ToList();

            await _jogoCache.DefinirTodosAsync(jogos);
            return jogos;
        }

        public async Task<JogoResponse?> ObterPorIdAsync(Guid id)
        {
            var cached = await _jogoCache.ObterPorIdAsync(id);
            if (cached is not null)
                return cached;

            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo is null)
                return null;

            var response = ToResponse(jogo);
            await _jogoCache.DefinirPorIdAsync(response);
            return response;
        }

        public async Task<JogoResponse> CriarAsync(JogoRequest request)
        {
            var jogo = new Jogo(request.Titulo, request.Descricao, request.Preco);
            await _jogoRepository.AdicionarAsync(jogo);
            await _jogoCache.InvalidarAsync(jogo.Id);
            return ToResponse(jogo);
        }

        public async Task<JogoResponse?> AtualizarAsync(Guid id, JogoRequest request)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo is null)
                return null;

            jogo.Titulo = request.Titulo;
            jogo.Descricao = request.Descricao;
            jogo.Preco = request.Preco;
            jogo.DataAtualizacao = DateTime.UtcNow;

            await _jogoRepository.AtualizarAsync(jogo);
            await _jogoCache.InvalidarAsync(id);
            return ToResponse(jogo);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var removido = await _jogoRepository.RemoverAsync(id);
            if (removido)
                await _jogoCache.InvalidarAsync(id);

            return removido;
        }

        private static JogoResponse ToResponse(Jogo jogo) => new()
        {
            Id = jogo.Id,
            Titulo = jogo.Titulo,
            Descricao = jogo.Descricao,
            DataCriacao = jogo.DataCriacao,
            DataAtualizacao = jogo.DataAtualizacao,
            Preco = jogo.Preco
        };
    }
}