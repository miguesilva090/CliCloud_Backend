using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ServicoTratamentoConfiguration : IEntityTypeConfiguration<ServicoTratamento>
  {
    public void Configure(EntityTypeBuilder<ServicoTratamento> builder)
    {
      builder.ToTable("ServicoTratamento", "Tratamentos");

      // Relacionamento N:1 com Tratamento
      builder.HasOne(s => s.Tratamento)
        .WithMany(t => t.Servicos)
        .HasForeignKey(s => s.TratamentoId)
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamento N:1 com Servico (opcional)
      builder.HasOne(s => s.Servico)
        .WithMany()
        .HasForeignKey(s => s.ServicoId)
        .OnDelete(DeleteBehavior.SetNull);

      // Evitar duplicação lógica: uma ordem por (Tratamento, Ordem); Ordem nullable (múltiplos NULL permitidos em SQL Server)
      builder.HasIndex(s => new { s.TratamentoId, s.Ordem })
        .IsUnique()
        .HasFilter("[Ordem] IS NOT NULL");
    }
  }
}
