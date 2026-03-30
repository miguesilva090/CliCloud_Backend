using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class MedicoExternoConfiguration : IEntityTypeConfiguration<MedicoExterno>
  {
    public void Configure(EntityTypeBuilder<MedicoExterno> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("MedicoExterno", "Medicos");
    }
  }
}
