using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class SeparadorVinculoConfiguration : IEntityTypeConfiguration<SeparadorVinculo>
{
    public void Configure(EntityTypeBuilder<SeparadorVinculo> builder)
    {
        builder.ToTable("SeparadorVinculo", "ProcessoClinico");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Tipo).IsRequired();

        builder.HasIndex(x => new { x.SeparadorId, x.Tipo, x.EntidadeId }).IsUnique();

        builder.HasOne(x => x.Separador)
            .WithMany()
            .HasForeignKey(x => x.SeparadorId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
