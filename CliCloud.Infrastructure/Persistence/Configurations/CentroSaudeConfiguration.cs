using CliCloud.Domain.Entities.CentroSaude;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class CentroSaudeConfiguration : IEntityTypeConfiguration<CentroSaude>
  {
    public void Configure(EntityTypeBuilder<CentroSaude> builder)
    {
      builder.ToTable("CentroSaude", "CentroSaude");
    }
  }
}
