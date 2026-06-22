using FiapCloudGames.Catalogs.Domain.Pedidos.Entities;

namespace FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces
{
    public interface IPedidoDomainService
    {
        Task<Pedido> CriarAsync(string nome, string email, Guid IdJogo);
    }
}
