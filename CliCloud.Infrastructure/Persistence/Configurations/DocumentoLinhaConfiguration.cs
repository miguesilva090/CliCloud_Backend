using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class DocumentoLinhaConfiguration : IEntityTypeConfiguration<DocumentoLinha>
{
  public void Configure(EntityTypeBuilder<DocumentoLinha> builder)
  {
    builder.ToTable("DocumentoLinha", "Documentos");

    builder.Property(l => l.ModuloOrigemLinha)
      .HasConversion<int>();

    builder.HasOne(l => l.Documento)
      .WithMany(d => d.Linhas)
      .HasForeignKey(l => l.DocumentoId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(l => l.Servico)
      .WithMany()
      .HasForeignKey(l => l.ServicoId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(l => l.Artigo)
      .WithMany()
      .HasForeignKey(l => l.ArtigoId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(l => l.TaxaIva)
      .WithMany()
      .HasForeignKey(l => l.TaxaIvaId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(l => l.AdmissaoServico)
      .WithMany()
      .HasForeignKey(l => l.AdmissaoServicoId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasIndex(l => new { l.DocumentoId, l.NumeroLinha })
      .IsUnique();

    builder.HasIndex(l => l.ServicoId);
    builder.HasIndex(l => l.ArtigoId);
    builder.HasIndex(l => l.TaxaIvaId);
  }
}
