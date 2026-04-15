using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ModeloDocumentoConfiguration : IEntityTypeConfiguration<ModeloDocumento>
{
    public void Configure(EntityTypeBuilder<ModeloDocumento> builder)
    {
        builder.ToTable("ModeloDocumento", "Documentos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClinicaId).IsRequired();
        builder.Property(x => x.Codigo).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(200);

        builder.Property(x => x.Tipo).IsRequired();
        builder.Property(x => x.Estado).IsRequired();
        builder.Property(x => x.ConteudoHtml).IsRequired();

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo, x.Versao }).IsUnique();

        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).HasMaxLength(50);
    }
}