using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaMotivoIsencaoDefaultConfiguration : IEntityTypeConfiguration<ClinicaMotivoIsencaoDefault>
  {
    public void Configure(EntityTypeBuilder<ClinicaMotivoIsencaoDefault> builder)
    {
      builder.ToTable("ClinicaMotivoIsencaoDefault", "Core");
      builder.HasIndex(x => new { x.ClinicaId, x.Codigo }).IsUnique();
    }
  }
}
