using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class SeparadorPersonalizadoVinculoConfiguration : IEntityTypeConfiguration<SeparadorPersonalizadoVinculo>
{
    public void Configure(EntityTypeBuilder<SeparadorPersonalizadoVinculo> builder)
    {
        builder.ToTable("SeparadorPersonalizadoVinculo", "ProcessoClinico");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Tipo)
            .IsRequired();

        builder.HasIndex(x => new { x.SeparadorPersonalizadoId, x.Tipo, x.EntidadeId })
            .IsUnique();

        builder.HasOne(x => x.SeparadorPersonalizado)
            .WithMany()
            .HasForeignKey(x => x.SeparadorPersonalizadoId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
