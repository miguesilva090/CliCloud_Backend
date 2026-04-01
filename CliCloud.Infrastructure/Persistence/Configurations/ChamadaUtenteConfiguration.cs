using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ChamadaUtenteConfiguration : IEntityTypeConfiguration<ChamadaUtente>
  {
    public void Configure(EntityTypeBuilder<ChamadaUtente> builder)
    {
      builder.ToTable("ChamadaUtente", "Core");

      builder.HasIndex(x => new { x.ClinicaId, x.Tipo, x.ReferenciaId, x.Estado });
      builder.HasIndex(x => new { x.ClinicaId, x.DataHoraChamada });

      builder.Property(x => x.Tipo).IsRequired().HasMaxLength(20);
      builder.Property(x => x.NomeUtente).IsRequired().HasMaxLength(200);
      builder.Property(x => x.NomeProfissional).HasMaxLength(200);
      builder.Property(x => x.Sala).HasMaxLength(50);
      builder.Property(x => x.Senha).HasMaxLength(50);
      builder.Property(x => x.Estado).HasDefaultValue(0);

      builder.HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
