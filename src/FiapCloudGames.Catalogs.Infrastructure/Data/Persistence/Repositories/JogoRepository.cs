using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using FiapCloudGames.Catalogs.Domain.Jogos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly CatalogsDbContext _context;

        public JogoRepository(CatalogsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Jogo>> ObterTodosAsync()
            => await _context.Jogos.AsNoTracking().ToListAsync();

        public async Task<Jogo?> ObterPorIdAsync(Guid id)
            => await _context.Jogos.FindAsync(id);

        public Task AdicionarAsync(Jogo jogo)
        {
            _context.Jogos.Add(jogo);
            return _context.SaveChangesAsync();
        }

        public Task AtualizarAsync(Jogo jogo)
        {
            _context.Jogos.Update(jogo);
            return _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var jogo = await _context.Jogos.FindAsync(id);
            if (jogo is null)
                return false;

            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
