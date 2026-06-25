using CliCloud.Domain.Entities.Faturacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ZonaComercialConfiguration : IEntityTypeConfiguration<ZonaComercial>
{
    public void Configure(EntityTypeBuilder<ZonaComercial> builder)
    {
        builder.ToTable("ZonaComercial", "Faturacao");

        builder.Property(x => x.Descricao).HasMaxLength(40);

        builder.HasIndex( x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex( x => x.ClinicaId);
    }
}