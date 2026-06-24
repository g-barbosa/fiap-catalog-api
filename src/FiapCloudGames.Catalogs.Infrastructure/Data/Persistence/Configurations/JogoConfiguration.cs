using FiapCloudGames.Catalogs.Domain.Jogos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Configurations
{
    public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Titulo).IsRequired().HasMaxLength(100);
            builder.Property(j => j.Descricao).IsRequired().HasMaxLength(300);
            builder.Property(j => j.DataCriacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(j => j.DataAtualizacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(j => j.Titulo).IsUnique();
        }
    }
}
