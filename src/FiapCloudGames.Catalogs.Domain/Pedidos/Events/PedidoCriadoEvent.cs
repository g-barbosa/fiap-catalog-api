namespace FiapCloudGames.Catalogs.Domain.Pedidos.Events
{
    public class PedidoCriadoEvent
    {
        public Guid PedidoId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public Guid IdJogo { get; set; }
        public Guid IdBiblioteca { get; set; }
    }
}
