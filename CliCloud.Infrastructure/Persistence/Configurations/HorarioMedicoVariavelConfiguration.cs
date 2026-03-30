using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class HorarioMedicoVariavelConfiguration : IEntityTypeConfiguration<HorarioMedicoVariavel>
  {
    public void Configure(EntityTypeBuilder<HorarioMedicoVariavel> builder)
    {
      builder.ToTable("HorarioMedicoVariavel", "Medicos");

      builder.HasOne(h => h.Medico)
        .WithMany()
        .HasForeignKey(h => h.MedicoId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasIndex(h => new { h.MedicoId, h.Data }).IsUnique();
    }
  }
}
