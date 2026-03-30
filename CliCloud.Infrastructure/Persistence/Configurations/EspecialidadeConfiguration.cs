using CliCloud.Domain.Entities.Especialidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class EspecialidadeConfiguration : IEntityTypeConfiguration<Especialidade>
  {
    public void Configure(EntityTypeBuilder<Especialidade> builder)
    {
      builder.ToTable("Especialidade", "Especialidades");

      // Relacionamento N:1 com CategoriaEspecialidade (opcional)
      builder.HasOne(e => e.CategoriaEspecialidade)
        .WithMany(c => c.Especialidades)
        .HasForeignKey(e => e.CategoriaEspecialidadeId)
        .OnDelete(DeleteBehavior.SetNull);
    }
  }
}
