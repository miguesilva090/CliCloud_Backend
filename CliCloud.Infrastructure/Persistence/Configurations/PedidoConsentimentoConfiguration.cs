using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class PedidoConsentimentoConfiguration : IEntityTypeConfiguration<PedidoConsentimento>
{
    public void Configure(EntityTypeBuilder<PedidoConsentimento> builder)
    {
        builder.ToTable("PedidoConsentimento", "Documentos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClinicaId).IsRequired();
        builder.Property(x => x.InstanciaDocumentoId).IsRequired();
        builder.Property(x => x.TipoConsentimento).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Canal).HasMaxLength(40);
        builder.Property(x => x.AssinadoPor).HasMaxLength(120);
        builder.Property(x => x.Observacoes).HasMaxLength(2000);

        builder.HasOne(x => x.InstanciaDocumento)
            .WithMany()
            .HasForeignKey(x => x.InstanciaDocumentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new {x.ClinicaId, x.Estado, x.CreatedOn});
        builder.HasIndex(x => new {x.ClinicaId, x.UtenteId, x.TipoConsentimento });
    }
}