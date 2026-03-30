using CliCloud.Domain.Entities.Doencas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class DoencaConfiguration : IEntityTypeConfiguration<Doenca>
  {
    public void Configure(EntityTypeBuilder<Doenca> builder)
    {
      builder
        .HasOne(e => e.Parent)
        .WithMany(e => e.Children)
        .HasForeignKey(e => e.ParentId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.Code).HasFilter("[Code] IS NOT NULL AND [Code] <> ''");
    }
  }
}
