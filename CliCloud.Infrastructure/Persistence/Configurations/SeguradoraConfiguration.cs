using CliCloud.Domain.Entities.Seguradoras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class SeguradoraConfiguration : IEntityTypeConfiguration<Seguradora>
  {
    public void Configure(EntityTypeBuilder<Seguradora> builder)
    {
      builder.ToTable("Seguradora", "Seguradoras");

      builder.HasOne(s => s.Banco)
        .WithMany()
        .HasForeignKey(s => s.BancoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
