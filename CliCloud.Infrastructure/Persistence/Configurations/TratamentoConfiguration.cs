using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TratamentoConfiguration : IEntityTypeConfiguration<Tratamento>
  {
    public void Configure(EntityTypeBuilder<Tratamento> builder)
    {
      builder.ToTable("Tratamento", "Tratamentos");

      // Relacionamentos N:1
      builder.HasOne(t => t.Utente)
        .WithMany()
        .HasForeignKey(t => t.UtenteId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.ListaEsperaTratamento)
        .WithMany()
        .HasForeignKey(t => t.ListaEsperaTratamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasIndex(t => t.ListaEsperaTratamentoId);

      builder.HasOne(t => t.Medico)
        .WithMany()
        .HasForeignKey(t => t.MedicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.Fisioterapeuta)
        .WithMany()
        .HasForeignKey(t => t.FisioterapeutaId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.Auxiliar)
        .WithMany()
        .HasForeignKey(t => t.AuxiliarId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.OutroTecnico)
        .WithMany()
        .HasForeignKey(t => t.OutroTecnicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.Organismo)
        .WithMany()
        .HasForeignKey(t => t.OrganismoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.LocalTratamento)
        .WithMany()
        .HasForeignKey(t => t.LocalTratamentoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(t => t.TratamentoPred)
        .WithMany()
        .HasForeignKey(t => t.TratamentoPredId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(t => t.Documento)
        .WithMany()
        .HasForeignKey(t => t.DocumentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(t => t.Recibo)
        .WithMany()
        .HasForeignKey(t => t.ReciboId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(t => t.Seguradora)
        .WithMany()
        .HasForeignKey(t => t.SeguradoraId)
        .OnDelete(DeleteBehavior.NoAction);

      // Relacionamentos 1:N com filhos
      builder.HasMany(t => t.Sessoes)
        .WithOne(s => s.Tratamento)
        .HasForeignKey(s => s.TratamentoId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(t => t.Servicos)
        .WithOne(s => s.Tratamento)
        .HasForeignKey(s => s.TratamentoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
