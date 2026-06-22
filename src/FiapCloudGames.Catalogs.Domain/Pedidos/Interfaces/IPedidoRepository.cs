using FiapCloudGames.Catalogs.Domain.Pedidos.Entities;

namespace FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);
        Task<Pedido?> ObterPorIdAsync(Guid pedidoId);
    }
}
