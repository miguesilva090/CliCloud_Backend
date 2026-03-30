using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioMedicoDiaConfiguration : IEntityTypeConfiguration<HorarioMedicoDia>
  {
    public void Configure(EntityTypeBuilder<HorarioMedicoDia> builder)
    {
      builder.ToTable("HorarioMedicoDia", "Medicos");

      // Relacionamento N:1 com HorarioMedico
      builder.HasOne(h => h.HorarioMedico)
        .WithMany(hm => hm.Horarios)
        .HasForeignKey(h => h.HorarioMedicoId)
        .OnDelete(DeleteBehavior.Cascade);

      // Índice único: não pode haver duplicatas de DiaSemana + Periodo para o mesmo HorarioMedico
      builder.HasIndex(h => new { h.HorarioMedicoId, h.DiaSemana, h.Periodo })
        .IsUnique();

      // Configurar enum conversions
      builder.Property(h => h.DiaSemana)
        .HasConversion<int>();

      builder.Property(h => h.Periodo)
        .HasConversion<int>();
    }
  }
}
