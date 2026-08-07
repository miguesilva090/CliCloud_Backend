using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class SessaoTratamentoConfiguration : IEntityTypeConfiguration<SessaoTratamento>
  {
    public void Configure(EntityTypeBuilder<SessaoTratamento> builder)
    {
      builder.ToTable("SessaoTratamento", "Tratamentos");

      // Relacionamento N:1 com Tratamento
      builder.HasOne(s => s.Tratamento)
        .WithMany(t => t.Sessoes)
        .HasForeignKey(s => s.TratamentoId)
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamentos N:1 com técnicos
      builder.HasOne(s => s.Fisioterapeuta)
        .WithMany()
        .HasForeignKey(s => s.FisioterapeutaId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(s => s.Auxiliar)
        .WithMany()
        .HasForeignKey(s => s.AuxiliarId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(s => s.OutroTecnico)
        .WithMany()
        .HasForeignKey(s => s.OutroTecnicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(s => s.TipoDocumento)
        .WithMany()
        .HasForeignKey(s => s.TipoDocumentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(s => s.Documento)
        .WithMany()
        .HasForeignKey(s => s.DocumentoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(s => s.Recibo)
        .WithMany()
        .HasForeignKey(s => s.ReciboId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(s => s.MotivoDesmarcacao)
        .WithMany()
        .HasForeignKey(s => s.MotivoDesmarcacaoId)
        .OnDelete(DeleteBehavior.SetNull);

      // Relacionamento 1:N com ServicoSessao
      builder.HasMany(s => s.Servicos)
        .WithOne(ss => ss.SessaoTratamento)
        .HasForeignKey(ss => ss.SessaoTratamentoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
