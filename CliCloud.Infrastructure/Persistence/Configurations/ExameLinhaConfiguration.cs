using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ExameLinhaConfiguration : IEntityTypeConfiguration<ExameLinha>
  {
    public void Configure(EntityTypeBuilder<ExameLinha> builder)
    {
      builder.ToTable("ExameLinha", "Exames");

      builder.HasOne(e => e.Exame)
        .WithMany(x => x.Linhas)
        .HasForeignKey(e => e.ExameId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(e => e.TipoExame)
        .WithMany()
        .HasForeignKey(e => e.TipoExameId)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
