using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class SalaConfiguration : IEntityTypeConfiguration<Sala>
  {
    public void Configure(EntityTypeBuilder<Sala> builder)
    {
      builder.ToTable("Sala", "Consultas");
      builder.HasOne(x => x.Clinica).WithMany().HasForeignKey(x => x.ClinicaId);
    }
  }
}
