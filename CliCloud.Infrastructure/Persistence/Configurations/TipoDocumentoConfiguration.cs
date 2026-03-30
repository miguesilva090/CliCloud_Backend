using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
  {
    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
      builder.ToTable("TipoDocumento", "Documentos");

      // Índice único na Abreviatura
      builder.HasIndex(t => t.Abreviatura)
        .IsUnique();
    }
  }
}
