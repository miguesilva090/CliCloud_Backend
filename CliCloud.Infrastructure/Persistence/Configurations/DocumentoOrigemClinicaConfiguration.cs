using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class DocumentoOrigemClinicaConfiguration : IEntityTypeConfiguration<DocumentoOrigemClinica>
{
  public void Configure(EntityTypeBuilder<DocumentoOrigemClinica> builder)
  {
    builder.ToTable("DocumentoOrigemClinica", "Documentos");

    builder.Property(o => o.ModuloOrigem)
      .HasConversion<int>();

    builder.HasOne(o => o.Documento)
      .WithOne(d => d.OrigemClinica)
      .HasForeignKey<DocumentoOrigemClinica>(o => o.DocumentoId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(o => o.Admissao)
      .WithMany()
      .HasForeignKey(o => o.AdmissaoId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(o => o.Consulta)
      .WithMany()
      .HasForeignKey(o => o.ConsultaId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasIndex(o => o.DocumentoId)
      .IsUnique();

    builder.HasIndex(o => o.AdmissaoId);
    builder.HasIndex(o => o.ConsultaId);
  }
}
