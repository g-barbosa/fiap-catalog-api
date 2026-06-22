namespace FiapCloudGames.Catalogs.Domain.Pedidos.Entities
{
    public class Pedido
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid IdJogo { get; set; }
    }
}
