using CliCloud.Domain.Entities.Medicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
  {
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Medico", "Medicos");

      // Relacionamento N:1 com Especialidade
      builder.HasOne(m => m.Especialidade)
        .WithMany()
        .HasForeignKey(m => m.EspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
