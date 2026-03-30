using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class MarcaAparelhoConfiguration : IEntityTypeConfiguration<MarcaAparelho>
  {
    public void Configure(EntityTypeBuilder<MarcaAparelho> builder)
    {
      builder.ToTable("MarcaAparelho", "Tratamentos");
    }
  }
}
