using FiapCloudGames.Catalogs.Domain.Pedidos.Entities;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces;

namespace FiapCloudGames.Catalogs.Domain.Pedidos.Services
{
    public class PedidoDomainService : IPedidoDomainService
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoDomainService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        public async Task<Pedido> CriarAsync(string nome, string email, Guid IdJogo)
        {
            var pedido = new Pedido
            {
                NomeUsuario = nome,
                Email = email,
                IdJogo = IdJogo
            };
            await _pedidoRepository.AdicionarAsync(pedido);

            return pedido;
        }
    }
}
