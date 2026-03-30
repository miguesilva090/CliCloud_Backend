using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaApiKeyConfiguration : IEntityTypeConfiguration<ClinicaApiKey>
  {
    public void Configure(EntityTypeBuilder<ClinicaApiKey> builder)
    {
      builder.HasKey(x => x.Id);

      builder.Property(x => x.ApiKey).IsRequired().HasMaxLength(512);

      builder.HasIndex(x => x.ApiKey).IsUnique();
      builder.HasIndex(x => new { x.ClinicaId, x.ApiKey }).IsUnique();

      builder
        .HasOne(x => x.Clinica)
        .WithMany()
        .HasForeignKey(x => x.ClinicaId);
    }
  }
}

