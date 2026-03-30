using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class PatologiaServicoConfiguration : IEntityTypeConfiguration<PatologiaServico>
  {
    public void Configure(EntityTypeBuilder<PatologiaServico> builder)
    {
      builder.ToTable("PatologiaServico", "Tratamentos");

      builder.HasOne(ps => ps.SubsistemaServico)
        .WithMany()
        .HasForeignKey(ps => ps.SubsistemaServicoId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
