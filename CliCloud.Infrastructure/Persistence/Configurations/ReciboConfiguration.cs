using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ReciboConfiguration : IEntityTypeConfiguration<Recibo>
  {
    public void Configure(EntityTypeBuilder<Recibo> builder)
    {
      builder.ToTable("Recibo", "Documentos");
    }
  }
}
