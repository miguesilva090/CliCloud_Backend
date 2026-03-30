using CliCloud.Domain.Entities.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class EntidadeConfiguration : IEntityTypeConfiguration<Entidade>
  {
    public void Configure(EntityTypeBuilder<Entidade> builder)
    {
      builder.Property(e => e.TipoEntidade)
        .IsRequired()
        .HasConversion<int>();
    }
  }
}
