using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoAparelhoConfiguration : IEntityTypeConfiguration<TipoAparelho>
  {
    public void Configure(EntityTypeBuilder<TipoAparelho> builder)
    {
      builder.ToTable("TipoAparelho", "Tratamentos");
      builder.HasIndex(x => x.Designacao);
    }
  }
}
