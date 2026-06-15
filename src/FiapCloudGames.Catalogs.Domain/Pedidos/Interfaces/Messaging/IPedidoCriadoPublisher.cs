using FiapCloudGames.Catalogs.Domain.Pedidos.Events;

namespace FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces.Messaging
{
    public interface IPedidoCriadoPublisher
    {
        Task PublicarPedidoCriadoAsync(PedidoCriadoEvent evento);
    }
}
