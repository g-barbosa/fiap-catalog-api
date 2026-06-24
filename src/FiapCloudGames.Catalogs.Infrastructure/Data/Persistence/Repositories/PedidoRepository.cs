using FiapCloudGames.Catalogs.Domain.Pedidos.Entities;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly CatalogsDbContext _context;

        public PedidoRepository(CatalogsDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid pedidoId)
        {
            return await _context.Pedidos.FindAsync(pedidoId);
        }
    }
}
