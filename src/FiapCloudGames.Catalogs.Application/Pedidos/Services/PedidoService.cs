using FiapCloudGames.Catalogs.Application.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces.Messaging;

namespace FiapCloudGames.Catalogs.Application.Pedidos.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDomainService _pedidoDomainService;
        private readonly IPedidoCriadoPublisher _pedidoCriadoPublisher;
        public PedidoService(IPedidoDomainService pedidoDomainService, IPedidoCriadoPublisher pedidoCriadoPublisher)
        {
            _pedidoDomainService = pedidoDomainService;
            _pedidoCriadoPublisher = pedidoCriadoPublisher;
        }
        public async Task ProcessarPedido()
        {
            var pedido = await _pedidoDomainService.CriarAsync("", "", Guid.NewGuid());
            await _pedidoCriadoPublisher.PublicarPedidoCriadoAsync(new Domain.Pedidos.Events.PedidoCriadoEvent
            {
                PedidoId = pedido.Id,
                NomeUsuario = "",
                Email = "",
                IdJogo = Guid.NewGuid()
            });
        }
    }
}
