using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class AparelhoConfiguration : IEntityTypeConfiguration<Aparelho>
  {
    public void Configure(EntityTypeBuilder<Aparelho> builder)
    {
      builder.ToTable("Aparelho", "Tratamentos");

      builder.HasOne(a => a.TipoAparelho)
        .WithMany(t => t.Aparelhos)
        .HasForeignKey(a => a.TipoAparelhoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(a => a.ModeloAparelho)
        .WithMany()
        .HasForeignKey(a => a.ModeloAparelhoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
