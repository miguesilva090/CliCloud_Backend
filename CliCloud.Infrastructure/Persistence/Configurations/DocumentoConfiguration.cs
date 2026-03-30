using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
  {
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Documento", "Documentos");

      // Configurar enum conversions
      builder.Property(d => d.CondicaoPagamento)
        .HasConversion<int>();

      builder.Property(d => d.TipoModoPagamento)
        .HasConversion<int>();

      // Relacionamentos N:1
      builder.HasOne(d => d.Utente)
        .WithMany()
        .HasForeignKey(d => d.UtenteId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(d => d.Organismo)
        .WithMany()
        .HasForeignKey(d => d.OrganismoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(d => d.Funcionario)
        .WithMany()
        .HasForeignKey(d => d.FuncionarioId)
        .OnDelete(DeleteBehavior.SetNull);

      // Relacionamento N:1 com TipoDocumento
      builder.HasOne(d => d.TipoDocumento)
        .WithMany()
        .HasForeignKey(d => d.TipoDocumentoId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(d => d.CodigoPostal)
        .WithMany()
        .HasForeignKey(d => d.CodigoPostalId)
        .OnDelete(DeleteBehavior.NoAction);

      // Índice único composto para garantir unicidade do documento
      builder.HasIndex(d => new { d.TipoDocumentoId, d.NumeroDocumento })
        .IsUnique();

      // Índices para melhor performance
      builder.HasIndex(d => d.Data);
      builder.HasIndex(d => d.UtenteId);
      builder.HasIndex(d => d.OrganismoId);
      builder.HasIndex(d => d.Estado);
    }
  }
}
