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

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
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

        /// <summary>Atualiza os dados de um jogo existente.</summary>
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
    }
}
