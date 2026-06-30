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
        public async Task<Pedido> CriarAsync(string nome, string email, Guid idBiblioteca, Guid idJogo)
        {
            var pedido = new Pedido
            {
                NomeUsuario = nome,
                Email = email,
                IdBiblioteca = idBiblioteca,
                IdJogo = idJogo
            };
            await _pedidoRepository.AdicionarAsync(pedido);

            return pedido;
        }
    }
}
