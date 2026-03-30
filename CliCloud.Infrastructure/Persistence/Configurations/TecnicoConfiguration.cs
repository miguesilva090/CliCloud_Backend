using CliCloud.Domain.Entities.Tecnicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TecnicoConfiguration : IEntityTypeConfiguration<Tecnico>
  {
    public void Configure(EntityTypeBuilder<Tecnico> builder)
    {
      // Configure TPT (Table Per Type) inheritance
      builder.ToTable("Tecnico", "Tecnicos");

      // Relacionamento N:1 com Especialidade
      builder.HasOne(t => t.Especialidade)
        .WithMany()
        .HasForeignKey(t => t.EspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
