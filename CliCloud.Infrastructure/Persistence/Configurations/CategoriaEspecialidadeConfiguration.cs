using CliCloud.Domain.Entities.Especialidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class CategoriaEspecialidadeConfiguration : IEntityTypeConfiguration<CategoriaEspecialidade>
  {
    public void Configure(EntityTypeBuilder<CategoriaEspecialidade> builder)
    {
      builder.ToTable("CategoriaEspecialidade", "Especialidades");
    }
  }
}
