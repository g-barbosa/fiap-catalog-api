using FiapCloudGames.Catalogs.Application.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Application.DTOs.Bibliotecas;
using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Bibliotecas.Entities;
using FiapCloudGames.Catalogs.Domain.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;

namespace FiapCloudGames.Catalogs.Application.Bibliotecas.Services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IPedidoService _pedidoService;
        private readonly IJogoRepository _jogoRepository;

        public BibliotecaService(IBibliotecaRepository bibliotecaRepository, IJogoRepository jogoRepository, IPedidoService pedidoService)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _jogoRepository = jogoRepository;
            _pedidoService = pedidoService;
        }

        public async Task<IEnumerable<BibliotecaResponse>> ObterTodosAsync()
        {
            var bibliotecas = await _bibliotecaRepository.ObterTodosAsync();
            return bibliotecas.Select(ToResponse);
        }

        public async Task<BibliotecaResponse?> ObterPorIdAsync(Guid id)
        {
            var biblioteca = await _bibliotecaRepository.ObterPorIdAsync(id);
            return biblioteca is null ? null : ToResponse(biblioteca);
        }

        public async Task<BibliotecaResponse?> ObterPorUsuarioIdAsync(Guid usuarioId)
        {
            var biblioteca = await _bibliotecaRepository.ObterPorUsuarioIdAsync(usuarioId);
            return biblioteca is null ? null : ToResponse(biblioteca);
        }

        public async Task<BibliotecaResponse> CriarAsync(BibliotecaRequest request)
        {
            var biblioteca = new Biblioteca(request.UsuarioId);
            await _bibliotecaRepository.AdicionarAsync(biblioteca);
            return ToResponse(biblioteca);
        }

        public async Task<string?> SolicitarAdicaoJogoAsync(Guid bibliotecaId, Guid jogoId, string nomeUsuario, string email)
        {
            var biblioteca = await _bibliotecaRepository.ObterPorIdAsync(bibliotecaId);
            if (biblioteca is null)
                return null;

            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo is null)
                return null;

            await _pedidoService.ProcessarPedido(nomeUsuario, email, biblioteca.Id, jogo.Id);

            return $"Pedido de adição do jogo '{jogo.Titulo}' à biblioteca do usuário '{nomeUsuario}' esta sendo processado.";

        }

        public async Task<BibliotecaResponse?> AdicionarJogoAsync(Guid bibliotecaId, Guid jogoId)
        {
            var biblioteca = await _bibliotecaRepository.ObterPorIdAsync(bibliotecaId);
            if (biblioteca is null)
                return null;
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo is null)
                return null;
            biblioteca.AdicionarJogo(jogo);
            await _bibliotecaRepository.AtualizarAsync(biblioteca);
            return ToResponse(biblioteca);
        }

        public async Task<BibliotecaResponse?> RemoverJogoAsync(Guid bibliotecaId, Guid jogoId)
        {
            var biblioteca = await _bibliotecaRepository.ObterPorIdAsync(bibliotecaId);
            if (biblioteca is null)
                return null;

            biblioteca.RemoverJogo(jogoId);
            await _bibliotecaRepository.AtualizarAsync(biblioteca);
            return ToResponse(biblioteca);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            return await _bibliotecaRepository.RemoverAsync(id);
        }

        private static BibliotecaResponse ToResponse(Biblioteca biblioteca) => new()
        {
            Id = biblioteca.Id,
            UsuarioId = biblioteca.UsuarioId,
            Jogos = biblioteca.Jogos.Select(ToJogoResponse),
            DataCriacao = biblioteca.DataCriacao,
            DataAtualizacao = biblioteca.DataAtualizacao
        };

        private static JogoResponse ToJogoResponse(Jogo jogo) => new()
        {
            Id = jogo.Id,
            Titulo = jogo.Titulo,
            Descricao = jogo.Descricao,
            Preco = jogo.Preco,
            DataCriacao = jogo.DataCriacao,
            DataAtualizacao = jogo.DataAtualizacao
        };
    }
}
