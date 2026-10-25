using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using FiapCloudGames.Catalogs.Application.Jogos.Services;
using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;

namespace FiapCloudGames.Catalogs.Tests.Services
{
    public class JogoServiceTests
    {
        private readonly Mock<IJogoRepository> _repository = new();
        private readonly Mock<IJogoCache> _cache = new();
        private readonly Mock<IJogoSearch> _search = new();
        private readonly JogoService _service;

        public JogoServiceTests()
        {
            _repository
                .Setup(r => r.AdicionarAsync(It.IsAny<Jogo>()))
                .Returns(Task.CompletedTask);
            _repository
                .Setup(r => r.AtualizarAsync(It.IsAny<Jogo>()))
                .Returns(Task.CompletedTask);
            _cache
                .Setup(c => c.InvalidarAsync(It.IsAny<Guid?>()))
                .Returns(Task.CompletedTask);
            _search
                .Setup(s => s.IndexarAsync(It.IsAny<JogoResponse>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _search
                .Setup(s => s.RemoverAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _service = new JogoService(_repository.Object, _cache.Object, _search.Object);
        }

        [Fact]
        public async Task Dado_JogoNovo_Quando_Criar_Entao_PersisteInvalidaCacheEIndexa()
        {
            Jogo? persistido = null;
            _repository
                .Setup(r => r.AdicionarAsync(It.IsAny<Jogo>()))
                .Callback<Jogo>(jogo => persistido = jogo)
                .Returns(Task.CompletedTask);

            var request = new JogoRequest
            {
                Titulo = "Hades",
                Descricao = "Roguelike",
                Preco = 79.90m
            };

            var response = await _service.CriarAsync(request);

            persistido.Should().NotBeNull();
            response.Id.Should().Be(persistido!.Id);
            response.Titulo.Should().Be("Hades");
            response.Preco.Should().Be(79.90m);

            _cache.Verify(c => c.InvalidarAsync(persistido.Id), Times.Once);
            _search.Verify(s => s.IndexarAsync(
                It.Is<JogoResponse>(jogo =>
                    jogo.Id == persistido.Id
                    && jogo.Titulo == "Hades"
                    && jogo.Descricao == "Roguelike"
                    && jogo.Preco == 79.90m),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Dado_JogoExistente_Quando_Atualizar_Entao_PersisteInvalidaCacheEReindexa()
        {
            var jogo = new Jogo("Hades", "Roguelike", 79.90m);
            _repository
                .Setup(r => r.ObterPorIdAsync(jogo.Id))
                .ReturnsAsync(jogo);

            var request = new JogoRequest
            {
                Titulo = "Hades II",
                Descricao = "Sequencia",
                Preco = 149.90m
            };

            var response = await _service.AtualizarAsync(jogo.Id, request);

            response.Should().NotBeNull();
            response!.Titulo.Should().Be("Hades II");
            response.Preco.Should().Be(149.90m);
            jogo.Titulo.Should().Be("Hades II");

            _repository.Verify(r => r.AtualizarAsync(jogo), Times.Once);
            _cache.Verify(c => c.InvalidarAsync(jogo.Id), Times.Once);
            _search.Verify(s => s.IndexarAsync(
                It.Is<JogoResponse>(indexado =>
                    indexado.Id == jogo.Id
                    && indexado.Titulo == "Hades II"
                    && indexado.Descricao == "Sequencia"
                    && indexado.Preco == 149.90m),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Dado_JogoInexistente_Quando_Atualizar_Entao_NaoIndexaNemInvalidaCache()
        {
            var id = Guid.NewGuid();
            _repository
                .Setup(r => r.ObterPorIdAsync(id))
                .ReturnsAsync((Jogo?)null);

            var response = await _service.AtualizarAsync(id, new JogoRequest { Titulo = "Nada" });

            response.Should().BeNull();
            _repository.Verify(r => r.AtualizarAsync(It.IsAny<Jogo>()), Times.Never);
            _cache.Verify(c => c.InvalidarAsync(It.IsAny<Guid?>()), Times.Never);
            _search.Verify(
                s => s.IndexarAsync(It.IsAny<JogoResponse>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Dado_JogoRemovido_Quando_Deletar_Entao_InvalidaCacheERemoveDoIndice()
        {
            var id = Guid.NewGuid();
            _repository.Setup(r => r.RemoverAsync(id)).ReturnsAsync(true);

            var removido = await _service.DeletarAsync(id);

            removido.Should().BeTrue();
            _cache.Verify(c => c.InvalidarAsync(id), Times.Once);
            _search.Verify(s => s.RemoverAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
