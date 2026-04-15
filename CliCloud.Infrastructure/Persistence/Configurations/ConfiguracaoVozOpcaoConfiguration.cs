using CliCloud.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ConfiguracaoVozOpcaoConfiguration : IEntityTypeConfiguration<ConfiguracaoVozOpcao>
  {
    public void Configure(EntityTypeBuilder<ConfiguracaoVozOpcao> builder)
    {
      builder.ToTable("ConfiguracaoVozOpcao", "Core");
      builder.HasIndex(x => new { x.Tipo, x.Codigo }).IsUnique();
      builder.HasIndex(x => new { x.Tipo, x.Ativo, x.Ordem });

      builder.Property(x => x.Tipo).IsRequired().HasMaxLength(20);
      builder.Property(x => x.Codigo).IsRequired().HasMaxLength(30);
      builder.Property(x => x.Descricao).IsRequired().HasMaxLength(120);
      builder.Property(x => x.Ativo).HasDefaultValue(true);
    }
  }
}
