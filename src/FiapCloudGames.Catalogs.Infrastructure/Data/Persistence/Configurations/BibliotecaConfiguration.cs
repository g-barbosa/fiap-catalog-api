using FiapCloudGames.Catalogs.Domain.Bibliotecas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Configurations
{
    public class BibliotecaConfiguration : IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.UsuarioId).IsRequired();
            builder.Property(b => b.DataCriacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(b => b.DataAtualizacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(b => b.UsuarioId).IsUnique();

            builder.HasMany(b => b.Jogos)
                .WithMany()
                .UsingEntity(j => j.ToTable("BibliotecaJogos"));
        }
    }
}
