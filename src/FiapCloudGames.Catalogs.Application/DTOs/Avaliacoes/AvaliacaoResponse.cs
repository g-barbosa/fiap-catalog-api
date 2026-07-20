namespace FiapCloudGames.Catalogs.Application.DTOs.Avaliacoes
{
    public class AvaliacaoResponse
    {
        public Guid Id { get; set; }
        public Guid JogoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}