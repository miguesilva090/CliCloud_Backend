using CliCloud.Domain.Entities.UnidadesLocaisSaude;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class UnidadesLocaisSaudeConfiguration : IEntityTypeConfiguration<UnidadesLocaisSaude>
  {
    public void Configure(EntityTypeBuilder<UnidadesLocaisSaude> builder)
    {
      builder.ToTable("UnidadesLocaisSaude", "UnidadesLocaisSaude");

      builder.Property(x => x.Codigo).IsRequired();
      builder.Property(x => x.Nome).IsRequired().HasMaxLength(200);
      builder.Property(x => x.Nif).HasMaxLength(50);

      // Garantia simples para a lookup (não impede o seeding; evita duplicados).
      builder.HasIndex(x => x.Codigo).IsUnique();
    }
  }
}

