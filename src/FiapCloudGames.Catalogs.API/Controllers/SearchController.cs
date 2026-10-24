using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Catalogs.API.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IJogoSearch _jogoSearch;

        public SearchController(IJogoSearch jogoSearch)
        {
            _jogoSearch = jogoSearch;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string? q, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { message = "Informe o termo de busca em q." });

            var jogos = await _jogoSearch.BuscarAsync(q, cancellationToken);
            return Ok(jogos);
        }
    }
}
