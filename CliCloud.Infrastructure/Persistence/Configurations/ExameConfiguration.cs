using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ExameConfiguration : IEntityTypeConfiguration<Exame>
  {
    public void Configure(EntityTypeBuilder<Exame> builder)
    {
      builder.ToTable("Exame", "Exames");
    }
  }
}
