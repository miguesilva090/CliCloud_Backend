using CliCloud.Domain.Entities.Bancos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class BancoConfiguration : IEntityTypeConfiguration<Banco>
  {
    public void Configure(EntityTypeBuilder<Banco> builder)
    {
      builder.ToTable("Banco", "Bancos");
    }
  }
}
