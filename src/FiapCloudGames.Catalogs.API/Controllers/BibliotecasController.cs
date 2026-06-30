using FiapCloudGames.Catalogs.Application.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Application.DTOs.Bibliotecas;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecasController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;

        public BibliotecasController(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var bibliotecas = await _bibliotecaService.ObterTodosAsync();
            return Ok(bibliotecas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var biblioteca = await _bibliotecaService.ObterPorIdAsync(id);

            if (biblioteca is null)
                return NotFound();

            return Ok(biblioteca);
        }

        [HttpGet("usuario/{usuarioId:guid}")]
        public async Task<IActionResult> ObterPorUsuarioId(Guid usuarioId)
        {
            var biblioteca = await _bibliotecaService.ObterPorUsuarioIdAsync(usuarioId);

            if (biblioteca is null)
                return NotFound();

            return Ok(biblioteca);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] BibliotecaRequest request)
        {
            var biblioteca = await _bibliotecaService.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = biblioteca.Id }, biblioteca);
        }

        [HttpPost("{id:guid}/jogos")]
        public async Task<IActionResult> SolicitarAdicaoJogo(Guid id, [FromBody] AdicionarJogoRequest request)
        {
            Console.WriteLine($"Solicitando adição do jogo {request.JogoId} à biblioteca {id} para o usuário {request.NomeUsuario} ({request.Email})");
            var biblioteca = await _bibliotecaService.SolicitarAdicaoJogoAsync(id, request.JogoId, request.NomeUsuario, request.Email);
            Console.WriteLine($"Resultado da solicitação: {biblioteca}");

            if (biblioteca is null)
                return NotFound();

            return Ok(biblioteca);
        }

        [HttpDelete("{id:guid}/jogos/{jogoId:guid}")]
        public async Task<IActionResult> RemoverJogo(Guid id, Guid jogoId)
        {
            var biblioteca = await _bibliotecaService.RemoverJogoAsync(id, jogoId);

            if (biblioteca is null)
                return NotFound();

            return Ok(biblioteca);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var deletado = await _bibliotecaService.DeletarAsync(id);

            if (!deletado)
                return NotFound();

            return NoContent();
        }
    }
}
