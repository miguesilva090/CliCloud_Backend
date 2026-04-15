using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class SeparadorPersonalizadoConfiguration : IEntityTypeConfiguration<SeparadorPersonalizado>
{
    public void Configure(EntityTypeBuilder<SeparadorPersonalizado> builder)
    {
        builder.ToTable("SeparadorPersonalizado", "ProcessoClinico");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeSeparador)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.UtilizadorId)
            .IsRequired();

        builder.Property(x => x.Ordem)
            .IsRequired();

        builder.Property(x => x.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(x => new { x.ClinicaId, x.UtilizadorId, x.Ordem });
        builder.HasIndex(x => new { x.ClinicaId, x.UtilizadorId, x.NomeSeparador });

        builder.HasOne(x => x.Formulario)
            .WithMany()
            .HasForeignKey(x => x.FormularioId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
