using FiapCloudGames.Catalogs.Application.DTOs.Jogos;

namespace FiapCloudGames.Catalogs.Application.DTOs.Bibliotecas
{
    public class BibliotecaResponse
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public IEnumerable<JogoResponse> Jogos { get; set; } = new List<JogoResponse>();
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
