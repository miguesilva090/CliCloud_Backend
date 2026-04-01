using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ConfiguracaoChamadaVozConfiguration : IEntityTypeConfiguration<ConfiguracaoChamadaVoz>
  {
    public void Configure(EntityTypeBuilder<ConfiguracaoChamadaVoz> builder)
    {
      builder.ToTable("ConfiguracaoChamadaVoz", "Core");

      builder.HasIndex(x => x.ClinicaId).IsUnique();
      builder.HasIndex(x => new { x.ClinicaId, x.Ativo });

      builder.Property(x => x.Language).HasDefaultValue("pt");
      builder.Property(x => x.Tld).HasDefaultValue("pt");

      builder.HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
