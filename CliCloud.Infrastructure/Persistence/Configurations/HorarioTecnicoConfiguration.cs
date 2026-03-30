using CliCloud.Domain.Entities.Tecnicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioTecnicoConfiguration : IEntityTypeConfiguration<HorarioTecnico>
  {
    public void Configure(EntityTypeBuilder<HorarioTecnico> builder)
    {
      builder.ToTable("HorarioTecnico", "Tecnicos");

      // Relacionamento 1:1 com Tecnico
      builder.HasOne(h => h.Tecnico)
        .WithOne(t => t.Horario)
        .HasForeignKey<HorarioTecnico>(h => h.TecnicoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
