using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ServicoSessaoConfiguration : IEntityTypeConfiguration<ServicoSessao>
  {
    public void Configure(EntityTypeBuilder<ServicoSessao> builder)
    {
      builder.ToTable("ServicoSessao", "Tratamentos");

      // Relacionamento N:1 com SessaoTratamento
      builder.HasOne(ss => ss.SessaoTratamento)
        .WithMany(s => s.Servicos)
        .HasForeignKey(ss => ss.SessaoTratamentoId)
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamentos N:1 com técnicos
      builder.HasOne(ss => ss.Fisioterapeuta)
        .WithMany()
        .HasForeignKey(ss => ss.FisioterapeutaId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(ss => ss.Auxiliar)
        .WithMany()
        .HasForeignKey(ss => ss.AuxiliarId)
        .OnDelete(DeleteBehavior.SetNull);

      // Relacionamento N:1 com Servico (opcional)
      builder.HasOne(ss => ss.Servico)
        .WithMany()
        .HasForeignKey(ss => ss.ServicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(ss => ss.Aparelho)
        .WithMany()
        .HasForeignKey(ss => ss.AparelhoId)
        .OnDelete(DeleteBehavior.NoAction);

      // Evitar duplicação lógica: uma ordem por (SessaoTratamento, Ordem); Ordem nullable
      builder.HasIndex(ss => new { ss.SessaoTratamentoId, ss.Ordem })
        .IsUnique()
        .HasFilter("[Ordem] IS NOT NULL");
    }
  }
}
