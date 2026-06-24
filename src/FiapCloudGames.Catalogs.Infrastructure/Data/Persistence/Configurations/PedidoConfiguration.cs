using FiapCloudGames.Catalogs.Domain.Pedidos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Persistence.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(p => p.IdJogo).IsRequired();
            builder.Property(j => j.NomeUsuario).IsRequired().HasMaxLength(300);
            builder.Property(j => j.Email).IsRequired().HasMaxLength(300);
            builder.Property(j => j.DataAtualizacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(j => j.DataCriacao).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
