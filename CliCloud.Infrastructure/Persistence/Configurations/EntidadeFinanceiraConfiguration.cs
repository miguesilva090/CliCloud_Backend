using CliCloud.Domain.Entities.EntidadesFinanceiras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class EntidadeFinanceiraConfiguration : IEntityTypeConfiguration<EntidadeFinanceira>
  {
    public void Configure(EntityTypeBuilder<EntidadeFinanceira> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("EntidadeFinanceira", "EntidadesFinanceiras");

      // Configurar enum conversion
      builder.Property(e => e.CondicaoSns)
        .HasConversion<int>();

      // Relacionamento N:1 com TipoEntidadeFinanceira
      builder.HasOne(e => e.TipoEntidadeFinanceira)
        .WithMany()
        .HasForeignKey(e => e.TipoEntidadeFinanceiraId)
        .OnDelete(DeleteBehavior.Restrict);

      // Unicidade Nome+PaisPrefixo é validada no service (MatchNomePais); em TPT não é possível índice único entre colunas de tabelas diferentes.
      builder.HasIndex(e => e.PaisPrefixo);
      builder.HasIndex(e => e.TipoEntidadeFinanceiraId);
    }
  }
}
