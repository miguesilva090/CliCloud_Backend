using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class HistoriaClinicaConfiguration : IEntityTypeConfiguration<HistoriaClinica>
{
  public void Configure(EntityTypeBuilder<HistoriaClinica> builder)
  {
    builder.ToTable("HistoriasClinicas", "HistoriaClinica");

    builder.HasOne(h => h.Utente)
      .WithMany()
      .HasForeignKey(h => h.UtenteId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired();

    builder.HasOne(h => h.Medico)
      .WithMany()
      .HasForeignKey(h => h.MedicoId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired();

    builder.HasOne(h => h.Especialidade)
      .WithMany()
      .HasForeignKey(h => h.EspecialidadeId)
      .OnDelete(DeleteBehavior.NoAction)
      .IsRequired(false);

    builder.Property(h => h.Obs)
      .IsRequired();
  }
}

