using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ServicoConsultaConfiguration : IEntityTypeConfiguration<ServicoConsulta>
  {
    public void Configure(EntityTypeBuilder<ServicoConsulta> builder)
    {
      builder.ToTable("ServicoConsulta", "Consultas");

      // Relacionamento N:1 com Consulta
      builder.HasOne(s => s.Consulta)
        .WithMany(c => c.Servicos)
        .HasForeignKey(s => s.ConsultaId)
        .OnDelete(DeleteBehavior.Cascade);

      // Relacionamento N:1 com Servico (opcional)
      builder.HasOne(s => s.Servico)
        .WithMany()
        .HasForeignKey(s => s.ServicoId)
        .OnDelete(DeleteBehavior.SetNull);

      builder.HasOne(s => s.Exame)
        .WithMany()
        .HasForeignKey(s => s.ExameId)
        .OnDelete(DeleteBehavior.NoAction);

      // Evitar duplicação lógica: uma linha por (Consulta, Linha)
      builder.HasIndex(s => new { s.ConsultaId, s.Linha })
        .IsUnique();
    }
  }
}
