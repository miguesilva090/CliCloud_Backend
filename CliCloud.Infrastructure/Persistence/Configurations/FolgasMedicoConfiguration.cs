using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class FolgasMedicoConfiguration : IEntityTypeConfiguration<FolgasMedico>
  {
    public void Configure(EntityTypeBuilder<FolgasMedico> builder)
    {
      builder.ToTable("FolgasMedico", "Medicos");

      builder.HasOne(f => f.Medico)
        .WithMany()
        .HasForeignKey(f => f.MedicoId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
