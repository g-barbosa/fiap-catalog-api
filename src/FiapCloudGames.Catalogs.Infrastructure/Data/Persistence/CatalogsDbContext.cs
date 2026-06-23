using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence
{
    public class CatalogsDbContext : DbContext
    {
        public CatalogsDbContext(DbContextOptions<CatalogsDbContext> options) : base(options)
        {
        }

        public DbSet<Jogo> Jogos => Set<Jogo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
