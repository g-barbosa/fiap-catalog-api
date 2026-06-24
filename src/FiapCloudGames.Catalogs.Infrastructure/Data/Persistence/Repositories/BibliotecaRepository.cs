using FiapCloudGames.Catalogs.Domain.Bibliotecas.Entities;
using FiapCloudGames.Catalogs.Domain.Bibliotecas.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Repositories
{
    public class BibliotecaRepository : IBibliotecaRepository
    {
        private readonly CatalogsDbContext _context;

        public BibliotecaRepository(CatalogsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Biblioteca>> ObterTodosAsync()
            => await _context.Bibliotecas
                .AsNoTracking()
                .Include(b => b.Jogos)
                .ToListAsync();

        public async Task<Biblioteca?> ObterPorIdAsync(Guid id)
            => await _context.Bibliotecas
                .Include(b => b.Jogos)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Biblioteca?> ObterPorUsuarioIdAsync(Guid usuarioId)
            => await _context.Bibliotecas
                .Include(b => b.Jogos)
                .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);

        public Task AdicionarAsync(Biblioteca biblioteca)
        {
            _context.Bibliotecas.Add(biblioteca);
            return _context.SaveChangesAsync();
        }

        public Task AtualizarAsync(Biblioteca biblioteca)
        {
            _context.Bibliotecas.Update(biblioteca);
            return _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            var biblioteca = await _context.Bibliotecas.FindAsync(id);
            if (biblioteca is null)
                return false;

            _context.Bibliotecas.Remove(biblioteca);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
