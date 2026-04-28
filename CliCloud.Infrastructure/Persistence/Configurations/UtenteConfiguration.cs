using CliCloud.Domain.Entities.Utentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class UtenteConfiguration : IEntityTypeConfiguration<Utente>
  {
    public void Configure(EntityTypeBuilder<Utente> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Utente", "Utentes");

      // Configure enum conversions
      builder.Property(u => u.TipoConsulta)
        .HasConversion<int>();

      builder.Property(u => u.TipoTaxaModeradora)
        .HasConversion<int>();

      builder.HasOne(u => u.GrupoSanguineo)
        .WithMany()
        .HasForeignKey(u => u.GrupoSanguineoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.ProvenienciaUtente)
        .WithMany()
        .HasForeignKey(u => u.ProvenienciaUtenteId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.Organismo)
        .WithMany()
        .HasForeignKey(u => u.OrganismoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.SeguradoraOrganismo)
        .WithMany()
        .HasForeignKey(u => u.SeguradoraId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.Empresa)
        .WithMany()
        .HasForeignKey(u => u.EmpresaId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.CentroSaude)
        .WithMany()
        .HasForeignKey(u => u.CentroSaudeId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.MedicoExterno)
        .WithMany()
        .HasForeignKey(u => u.MedicoExternoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(u => u.Medico)
        .WithMany()
        .HasForeignKey(u => u.MedicoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.Property(u => u.CCValidado)
        .HasConversion<int>();

      // Indexes
      builder.HasIndex(u => u.NumeroUtente);
      builder.HasIndex(u => u.NumeroSegurancaSocial);
      builder.HasIndex(u => u.IdUtilizador);
    }
  }
}
