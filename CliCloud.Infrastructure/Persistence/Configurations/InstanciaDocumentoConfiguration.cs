using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class InstanciaDocumentoConfiguration : IEntityTypeConfiguration<InstanciaDocumento>
{
    public void Configure(EntityTypeBuilder<InstanciaDocumento> builder)
    {
        builder.ToTable("InstanciaDocumento", "Documentos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClinicaId).IsRequired();
        builder.Property(x => x.ModeloDocumentoId).IsRequired();
        builder.Property(x => x.VersaoModelo).IsRequired();

        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(300);
        builder.Property(x => x.ConteudoHtml).IsRequired();

        builder.Property(x => x.Assinado).IsRequired();
        builder.Property(x => x.AssinadoPor).HasMaxLength(100);

        builder.HasOne(x => x.ModeloDocumento)
            .WithMany()
            .HasForeignKey(x => x.ModeloDocumentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new { x.ClinicaId, x.ModeloDocumentoId, x.CreatedOn });

        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).HasMaxLength(50);
    }
}