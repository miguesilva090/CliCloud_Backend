using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ClinicaConfiguration : IEntityTypeConfiguration<Clinica>
  {
    public void Configure(EntityTypeBuilder<Clinica> builder)
    {
      builder.ToTable("Clinica", "Core");

      // Textos longos (política de privacidade, declarações)
      _ = builder.Property(c => c.RgpdDescritivo).HasColumnType("nvarchar(max)");
      _ = builder.Property(c => c.RgpdConsentimento).HasColumnType("nvarchar(max)");
      _ = builder.Property(c => c.RgpdMarketing).HasColumnType("nvarchar(max)");

      _ = builder.Property(c => c.EmailConteudoConsultas).HasColumnType("nvarchar(max)");
      _ = builder.Property(c => c.EmailConteudoTratamentos).HasColumnType("nvarchar(max)");
      _ = builder.Property(c => c.EmailConteudoExames).HasColumnType("nvarchar(max)");
      _ = builder.Property(c => c.EmailConteudoRelatorios).HasColumnType("nvarchar(max)");
    }
  }
}
