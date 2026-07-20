using FiapCloudGames.Catalogs.Application.Avaliacoes.Interfaces;
using FiapCloudGames.Catalogs.Application.DTOs.Avaliacoes;
using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;
        private readonly IAvaliacaoService _avaliacaoService;

        public JogosController(IJogoService jogoService, IAvaliacaoService avaliacaoService)
        {
            _jogoService = jogoService;
            _avaliacaoService = avaliacaoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var jogos = await _jogoService.ObterTodosAsync();
            return Ok(jogos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var jogo = await _jogoService.ObterPorIdAsync(id);

            if (jogo is null)
                return NotFound();

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] JogoRequest request)
        {
            var jogo = await _jogoService.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = jogo.Id }, jogo);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] JogoRequest request)
        {
            var jogo = await _jogoService.AtualizarAsync(id, request);

            if (jogo is null)
                return NotFound();

            return Ok(jogo);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var deletado = await _jogoService.DeletarAsync(id);

            if (!deletado)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id:guid}/avaliacoes")]
        public async Task<IActionResult> ObterAvaliacoes(Guid id)
        {
            var avaliacoes = await _avaliacaoService.ObterPorJogoIdAsync(id);

            if (avaliacoes is null)
                return NotFound();

            return Ok(avaliacoes);
        }

        [HttpPost("{id:guid}/avaliacoes")]
        public async Task<IActionResult> CriarAvaliacao(Guid id, [FromBody] AvaliacaoRequest request)
        {
            var avaliacao = await _avaliacaoService.CriarAsync(id, request);

            if (avaliacao is null)
                return NotFound();

            return CreatedAtAction(nameof(ObterAvaliacoes), new { id }, avaliacao);
        }
    }
}