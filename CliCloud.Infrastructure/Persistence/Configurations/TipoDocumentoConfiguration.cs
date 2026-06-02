using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
  {
    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
      builder.ToTable("TipoDocumento", "Documentos");

      builder.HasOne(t => t.Clinica)
        .WithMany()
        .HasForeignKey(t => t.ClinicaId)
        .OnDelete(DeleteBehavior.Restrict);

      // Série fiscal única por clínica (legado: unicidade em NumeroSerie por empresa)
      builder.HasIndex(t => new { t.ClinicaId, t.NumeroSerie })
        .IsUnique();

      builder.HasIndex(t => new { t.ClinicaId, t.Abreviatura });

      builder.HasIndex(t => t.ClinicaId);
    }
  }
}
