using CliCloud.Domain.Entities.Utentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class UtenteSubsistemaLinhaConfiguration : IEntityTypeConfiguration<UtenteSubsistemaLinha>
  {
    public void Configure(EntityTypeBuilder<UtenteSubsistemaLinha> builder)
    {
      builder.ToTable("UtenteSubsistemaLinha", "Utentes");

      builder.HasOne(x => x.Utente)
        .WithMany(u => u.SubsistemaLinhas)
        .HasForeignKey(x => x.UtenteId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(x => x.Organismo)
        .WithMany()
        .HasForeignKey(x => x.OrganismoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(x => x.Empresa)
        .WithMany()
        .HasForeignKey(x => x.EmpresaId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);
    }
  }
}
