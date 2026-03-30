using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ModeloAparelhoConfiguration : IEntityTypeConfiguration<ModeloAparelho>
  {
    public void Configure(EntityTypeBuilder<ModeloAparelho> builder)
    {
      builder.ToTable("ModeloAparelho", "Tratamentos");

      builder.HasOne(m => m.MarcaAparelho)
        .WithMany()
        .HasForeignKey(m => m.MarcaAparelhoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
