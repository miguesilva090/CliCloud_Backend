using CliCloud.Domain.Entities.Tecnicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioTecnicoVariavelConfiguration : IEntityTypeConfiguration<HorarioTecnicoVariavel>
  {
    public void Configure(EntityTypeBuilder<HorarioTecnicoVariavel> builder)
    {
      builder.ToTable("HorarioTecnicoVariavel", "Tecnicos");

      builder.HasOne(h => h.Tecnico)
        .WithMany()
        .HasForeignKey(h => h.TecnicoId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasIndex(h => new { h.TecnicoId, h.Data }).IsUnique();
    }
  }
}

