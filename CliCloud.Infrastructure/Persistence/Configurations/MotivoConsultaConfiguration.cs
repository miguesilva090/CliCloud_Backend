using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class MotivoConsultaConfiguration : IEntityTypeConfiguration<MotivoConsulta>
  {
    public void Configure(EntityTypeBuilder<MotivoConsulta> builder)
    {
      builder.ToTable("MotivosConsulta", "Consultas");
    }
  }
}
