namespace FiapCloudGames.Catalogs.Application.DTOs.Bibliotecas
{
    public class AdicionarJogoRequest
    {
        public Guid JogoId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
