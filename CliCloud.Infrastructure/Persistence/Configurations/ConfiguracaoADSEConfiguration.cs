using CliCloud.Domain.Entities.ConfiguracaoADSE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ConfiguracaoADSEConfiguration : IEntityTypeConfiguration<ConfiguracaoADSE>
{
  public void Configure(EntityTypeBuilder<ConfiguracaoADSE> builder)
  {
    _ = builder.Property(x => x.UrlADSE).HasMaxLength(500);
    _ = builder.Property(x => x.Dominio).HasMaxLength(255);
    _ = builder.Property(x => x.Utilizador).HasMaxLength(100);
    _ = builder.Property(x => x.Password).HasMaxLength(255);
    _ = builder.Property(x => x.PasswordLocal).HasMaxLength(255);
    _ = builder.Property(x => x.UrlPasta).HasMaxLength(500);

    _ = builder.HasIndex(x => x.EmpresaId).IsUnique();
  }
}
