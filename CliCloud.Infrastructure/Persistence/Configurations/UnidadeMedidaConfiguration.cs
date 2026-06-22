using CliCloud.Domain.Entities.Stocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class UnidadeMedidaConfiguration : IEntityTypeConfiguration<UnidadeMedida>
{
    public void Configure(EntityTypeBuilder<UnidadeMedida> builder)
    {
        builder.ToTable("UnidadeMedida", "Stocks");

        builder.Property(x => x.Descricao).HasMaxLength(15);

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => x.ClinicaId);
    }
}
