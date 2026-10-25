using FiapCloudGames.Catalogs.API.Controllers;
using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalogs.Tests.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<IJogoSearch> _search = new();
        private readonly SearchController _controller;

        public SearchControllerTests()
        {
            _controller = new SearchController(_search.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Dado_TermoVazio_Quando_Buscar_Entao_DeveRetornar400(string? termo)
        {
            var resultado = await _controller.Buscar(termo, CancellationToken.None);

            resultado.Should().BeOfType<BadRequestObjectResult>();
            _search.Verify(
                s => s.BuscarAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Dado_Termo_Quando_Buscar_Entao_DeveDevolverAListaNaOrdemDoMotor()
        {
            var ordenadosPorRelevancia = new List<JogoBuscaResponse>
            {
                new() { Id = Guid.NewGuid(), Titulo = "Hades", Descricao = "Roguelike", Preco = 79.90m, Score = 3.2 },
                new() { Id = Guid.NewGuid(), Titulo = "Hades II", Descricao = "Sequencia", Preco = 149.90m, Score = 1.4 }
            };

            _search
                .Setup(s => s.BuscarAsync("hads", It.IsAny<CancellationToken>()))
                .ReturnsAsync(ordenadosPorRelevancia);

            var resultado = await _controller.Buscar("hads", CancellationToken.None);

            var ok = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var jogos = ok.Value.Should().BeAssignableTo<IReadOnlyList<JogoBuscaResponse>>().Subject;

            jogos.Select(jogo => jogo.Titulo).Should().Equal("Hades", "Hades II");
            jogos.Select(jogo => jogo.Score).Should().BeInDescendingOrder();
            _search.Verify(s => s.BuscarAsync("hads", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
