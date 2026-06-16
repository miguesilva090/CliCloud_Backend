using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
  public void Configure(EntityTypeBuilder<Documento> builder)
  {
    builder.ToTable("Documento", "Documentos");

    builder.Property(d => d.EstadoDocumento)
      .HasConversion<int>();

    builder.Property(d => d.ModuloOrigem)
      .HasConversion<int>();

    builder.HasOne(d => d.Clinica)
      .WithMany()
      .HasForeignKey(d => d.ClinicaId)
      .OnDelete(DeleteBehavior.Restrict);

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

    builder.HasOne(d => d.TipoDocumento)
      .WithMany()
      .HasForeignKey(d => d.TipoDocumentoId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(d => d.CodigoPostal)
      .WithMany()
      .HasForeignKey(d => d.CodigoPostalId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(d => d.Moeda)
      .WithMany()
      .HasForeignKey(d => d.MoedaId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(d => d.Banco)
      .WithMany()
      .HasForeignKey(d => d.BancoId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(d => d.CondicaoPagamento)
      .WithMany()
      .HasForeignKey(d => d.CondicaoPagamentoId)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasOne(d => d.ModoPagamento)
      .WithMany()
      .HasForeignKey(d => d.ModoPagamentoId)
      .OnDelete(DeleteBehavior.SetNull);

    builder.HasOne(d => d.DocumentoOrigem)
      .WithMany(d => d.DocumentosDerivados)
      .HasForeignKey(d => d.DocumentoOrigemId)
      .OnDelete(DeleteBehavior.NoAction);

    // Numeração única por clínica, tipo, ano fiscal e número sequencial
    builder.HasIndex(d => new { d.ClinicaId, d.TipoDocumentoId, d.AnoFiscal, d.NumeroDocumento })
      .IsUnique();

    builder.HasIndex(d => d.Data);
    builder.HasIndex(d => d.UtenteId);
    builder.HasIndex(d => d.OrganismoId);
    builder.HasIndex(d => d.Estado);
    builder.HasIndex(d => d.EstadoDocumento);
    builder.HasIndex(d => d.ClinicaId);
    builder.HasIndex(d => d.NumeroExibicao);
    builder.HasIndex(d => d.DocumentoOrigemId);
    builder.HasIndex(d => d.CondicaoPagamentoId);
    builder.HasIndex(d => d.ModoPagamentoId);
  }
}
