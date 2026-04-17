using CliCloud.Domain.Entities.Core.ConfigReferenciaMB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfigReferenciaMbConfiguration : IEntityTypeConfiguration<ConfigReferenciaMB>
{
    public void Configure(EntityTypeBuilder<ConfigReferenciaMB> builder)
    {
        _ = builder.Property(x => x.ServicoUrl).HasMaxLength(255);
        _ = builder.Property(x => x.CodigoEntidade).HasMaxLength(50);
        _ = builder.Property(x => x.SubEntidade).HasMaxLength(50);
        _ = builder.Property(x => x.ChaveBackOffice).HasMaxLength(255);
        _ = builder.Property(x => x.IfThenKey).HasMaxLength(500);

        _ = builder.Property(x => x.ValorMinimo).HasPrecision(18, 2);
        _ = builder.HasIndex(x => x.ClinicaId).IsUnique();
    }
}