using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaTipoConsultaDefaultConfiguration : IEntityTypeConfiguration<ClinicaTipoConsultaDefault>
  {
    public void Configure(EntityTypeBuilder<ClinicaTipoConsultaDefault> builder)
    {
      builder.ToTable("ClinicaTipoConsultaDefault", "Core");
      builder.HasIndex(x => new { x.ClinicaId, x.Designacao }).IsUnique();
    }
  }
}
