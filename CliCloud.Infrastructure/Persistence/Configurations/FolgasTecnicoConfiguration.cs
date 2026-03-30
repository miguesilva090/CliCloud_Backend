using CliCloud.Domain.Entities.Tecnicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class FolgasTecnicoConfiguration : IEntityTypeConfiguration<FolgasTecnico>
  {
    public void Configure(EntityTypeBuilder<FolgasTecnico> builder)
    {
      builder.ToTable("FolgasTecnico", "Tecnicos");

      builder.HasOne(f => f.Tecnico)
        .WithMany()
        .HasForeignKey(f => f.TecnicoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}

