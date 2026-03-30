using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioMedicoConfiguration : IEntityTypeConfiguration<HorarioMedico>
  {
    public void Configure(EntityTypeBuilder<HorarioMedico> builder)
    {
      builder.ToTable("HorarioMedico", "Medicos");

      // Relacionamento 1:1 com Medico
      builder.HasOne(h => h.Medico)
        .WithOne(m => m.Horario)
        .HasForeignKey<HorarioMedico>(h => h.MedicoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
