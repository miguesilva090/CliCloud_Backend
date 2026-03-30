using CliCloud.Domain.Entities.Tecnicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioTecnicoDiaConfiguration : IEntityTypeConfiguration<HorarioTecnicoDia>
  {
    public void Configure(EntityTypeBuilder<HorarioTecnicoDia> builder)
    {
      builder.ToTable("HorarioTecnicoDia", "Tecnicos");

      // Relacionamento N:1 com HorarioTecnico
      builder.HasOne(h => h.HorarioTecnico)
        .WithMany(ht => ht.Horarios)
        .HasForeignKey(h => h.HorarioTecnicoId)
        .OnDelete(DeleteBehavior.Cascade);

      // Índice único: não pode haver duplicatas de DiaSemana + Periodo para o mesmo HorarioTecnico
      builder.HasIndex(h => new { h.HorarioTecnicoId, h.DiaSemana, h.Periodo })
        .IsUnique();

      // Configurar enum conversions
      builder.Property(h => h.DiaSemana)
        .HasConversion<int>();

      builder.Property(h => h.Periodo)
        .HasConversion<int>();
    }
  }
}
