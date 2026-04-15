using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class FicheiroDocumentoConfiguration : IEntityTypeConfiguration<FicheiroDocumento>
{
    public void Configure(EntityTypeBuilder<FicheiroDocumento> builder)
    {
        builder.ToTable("FicheiroDocumento", "Documentos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClinicaId).IsRequired();
        builder.Property(x => x.InstanciaDocumentoId).IsRequired();

        builder.Property(x => x.NomeOriginal).IsRequired().HasMaxLength(255);
        builder.Property(x => x.NomeArmazenamento).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CaminhoRelativo).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.TipoMime).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TamanhoBytes).IsRequired();

        builder.Property(x => x.ChecksumSha256).HasMaxLength(128);

        builder.HasOne(x => x.InstanciaDocumento)
            .WithMany()
            .HasForeignKey(x => x.InstanciaDocumentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new { x.ClinicaId, x.InstanciaDocumentoId });

        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.LastModifiedBy).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).HasMaxLength(50);
    }
}