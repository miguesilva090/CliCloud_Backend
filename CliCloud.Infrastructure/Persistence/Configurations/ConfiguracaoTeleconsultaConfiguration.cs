using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ConfiguracaoTeleconsultaConfiguration : IEntityTypeConfiguration<ConfiguracaoTeleconsulta>
  {
    public void Configure(EntityTypeBuilder<ConfiguracaoTeleconsulta> builder)
    {
      builder.ToTable("ConfiguracaoTeleconsulta", "Core");

      builder.HasIndex(x => x.ClinicaId).IsUnique();
      builder.HasIndex(x => new { x.ClinicaId, x.Ativo });

      builder.Property(x => x.Provider).IsRequired().HasMaxLength(50).HasDefaultValue("jitsi");
      builder.Property(x => x.BaseMeetingUrl).IsRequired().HasMaxLength(300).HasDefaultValue("https://meet.jit.si");
      builder.Property(x => x.JwtAppId).HasMaxLength(120);
      builder.Property(x => x.JwtApiKey).HasMaxLength(120);
      builder.Property(x => x.JwtKid).HasMaxLength(120);
      builder.Property(x => x.JwtPrivateKey).HasMaxLength(4000);
      builder.Property(x => x.JanelaEntradaMinutosAntes).HasDefaultValue(15);
      builder.Property(x => x.DuracaoPadraoMinutos).HasDefaultValue(30);
      builder.Property(x => x.PermitirEntradaAntesDoInicio).HasDefaultValue(true);
      builder.Property(x => x.LobbyAtivo).HasDefaultValue(true);

      builder.HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
