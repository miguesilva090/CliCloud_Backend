using CliCloud.Domain.Entities.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoServicoConfiguration : IEntityTypeConfiguration<TipoServico>
  {
    public void Configure(EntityTypeBuilder<TipoServico> builder)
    {
      builder.ToTable("TipoServico", "Servicos");

      // Índice na Descricao para pesquisas
      builder.HasIndex(t => t.Descricao);
    }
  }
}
