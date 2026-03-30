using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class PatologiaDoencaConfiguration : IEntityTypeConfiguration<PatologiaDoenca>
  {
    public void Configure(EntityTypeBuilder<PatologiaDoenca> builder)
    {
      builder.ToTable("PatologiaDoenca", "Tratamentos");

      builder.HasKey(x => new { x.PatologiaId, x.DoencaId });

      builder.HasOne(pd => pd.Doenca)
        .WithMany()
        .HasForeignKey(pd => pd.DoencaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
