using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaConfiguracaoIvaConfiguration : IEntityTypeConfiguration<ClinicaConfiguracaoIva>
  {
    public void Configure(EntityTypeBuilder<ClinicaConfiguracaoIva> builder)
    {
      builder.ToTable("ClinicaConfiguracaoIva", "Core");
      builder.HasIndex(x => new { x.ClinicaId, x.Ano }).IsUnique();
    }
  }
}
