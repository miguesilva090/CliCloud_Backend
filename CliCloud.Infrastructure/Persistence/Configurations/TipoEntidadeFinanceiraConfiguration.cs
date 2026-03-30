using CliCloud.Domain.Entities.TipoEntidadeFinanceira;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoEntidadeFinanceiraConfiguration : IEntityTypeConfiguration<TipoEntidadeFinanceira>
  {
    public void Configure(EntityTypeBuilder<TipoEntidadeFinanceira> builder)
    {
      builder.ToTable("TipoEntidadeFinanceira", "TipoEntidadeFinanceira");

      // Índice único na Sigla
      builder.HasIndex(t => t.Sigla)
        .IsUnique();
    }
  }
}
