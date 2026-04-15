using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TeleconsultaSessaoConfiguration : IEntityTypeConfiguration<TeleconsultaSessao>
  {
    public void Configure(EntityTypeBuilder<TeleconsultaSessao> builder)
    {
      builder.ToTable("TeleconsultaSessao", "Consultas");

      builder.HasIndex(x => x.MeetingId).IsUnique();
      builder.HasIndex(x => new { x.ClinicaId, x.ConsultaMarcacaoId, x.Ativo });

      builder.Property(x => x.MeetingId).IsRequired().HasMaxLength(120);
      builder.Property(x => x.MeetingUrl).IsRequired().HasMaxLength(500);
      builder.Property(x => x.Provider).IsRequired().HasMaxLength(50).HasDefaultValue("jitsi");
      builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Criada");
      builder.Property(x => x.TokenMedico).HasMaxLength(1024);
      builder.Property(x => x.TokenUtente).HasMaxLength(1024);
      builder.Property(x => x.LinksAtivos).HasDefaultValue(true);

      builder.HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(x => x.ConsultaMarcacao)
        .WithMany()
        .HasForeignKey(x => x.ConsultaMarcacaoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
