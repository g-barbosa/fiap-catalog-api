using FiapCloudGames.Catalogs.Domain.Core;

namespace FiapCloudGames.Catalogs.Domain.Pedidos.Entities
{
    public class Pedido : EntityBase
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid IdJogo { get; set; }
        public Guid IdBiblioteca { get; set; }
    }
}
