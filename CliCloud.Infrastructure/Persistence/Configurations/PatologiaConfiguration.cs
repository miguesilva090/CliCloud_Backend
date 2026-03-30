using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class PatologiaConfiguration : IEntityTypeConfiguration<Patologia>
  {
    public void Configure(EntityTypeBuilder<Patologia> builder)
    {
      builder.ToTable("Patologias", "Tratamentos");

      builder.HasOne(p => p.LocalTratamento)
        .WithMany()
        .HasForeignKey(p => p.LocalTratamentoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasOne(p => p.Organismo)
        .WithMany()
        .HasForeignKey(p => p.OrganismoId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

      builder.HasMany(p => p.PatologiaServicos)
        .WithOne(ps => ps.Patologia)
        .HasForeignKey(ps => ps.PatologiaId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasMany(p => p.PatologiaDoencas)
        .WithOne(pd => pd.Patologia)
        .HasForeignKey(pd => pd.PatologiaId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
