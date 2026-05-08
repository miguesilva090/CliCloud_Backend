using CliCloud.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoCartaConfiguration : IEntityTypeConfiguration<TipoCarta>
  {
    public void Configure(EntityTypeBuilder<TipoCarta> builder)
    {
      builder.ToTable("TiposCarta", "Comum");
    }
  }
}
