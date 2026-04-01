using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaArmazemDefaultConfiguration : IEntityTypeConfiguration<ClinicaArmazemDefault>
  {
    public void Configure(EntityTypeBuilder<ClinicaArmazemDefault> builder)
    {
      builder.ToTable("ClinicaArmazemDefault", "Core");
      builder.HasIndex(x => x.ClinicaId).IsUnique();
    }
  }
}
