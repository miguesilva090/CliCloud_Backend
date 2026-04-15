using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ConfiguracaoVozConfiguration : IEntityTypeConfiguration<ConfiguracaoVoz>
  {
    public void Configure(EntityTypeBuilder<ConfiguracaoVoz> builder)
    {
      builder.ToTable("ConfiguracaoVoz", "Core");

      builder.HasIndex(x => x.ClinicaId).IsUnique();
      builder.HasIndex(x => new { x.ClinicaId, x.Ativo });

      builder.Property(x => x.Provider).IsRequired().HasMaxLength(50).HasDefaultValue("web-speech");
      builder.Property(x => x.IdiomaPadrao).IsRequired().HasMaxLength(10).HasDefaultValue("pt-PT");
      builder.Property(x => x.SttIdioma).IsRequired().HasMaxLength(10).HasDefaultValue("pt-PT");
      builder.Property(x => x.TtsVoice).HasMaxLength(100);

      builder.Property(x => x.SttConfidenceMin).HasPrecision(5, 4).HasDefaultValue(0.5000m);
      builder.Property(x => x.SttSilenceTimeoutMs).HasDefaultValue(2500);
      builder.Property(x => x.SttMaxAlternatives).HasDefaultValue(1);
      builder.Property(x => x.SttProfanityFilter).HasDefaultValue(false);
      builder.Property(x => x.TtsRate).HasPrecision(5, 2).HasDefaultValue(1.00m);
      builder.Property(x => x.TtsPitch).HasPrecision(5, 2).HasDefaultValue(1.00m);
      builder.Property(x => x.TtsVolume).HasPrecision(5, 2).HasDefaultValue(1.00m);

      builder.Property(x => x.TimeoutMs).HasDefaultValue(15000);
      builder.Property(x => x.MaxDuracaoCapturaSegundos).HasDefaultValue(90);

      builder.HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
