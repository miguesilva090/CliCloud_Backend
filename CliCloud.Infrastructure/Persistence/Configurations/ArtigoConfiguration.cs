using CliCloud.Domain.Entities.Stocks;
using CliCloud.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ArtigoConfiguration : IEntityTypeConfiguration<Artigo>
{
    public void Configure(EntityTypeBuilder<Artigo> builder)
    {
        builder.ToTable("Artigo", "Stocks");

        builder.Property(x => x.NumeroArtigo).HasMaxLength(20);
        builder.Property(x => x.Descricao).HasMaxLength(100);
        builder.Property(x => x.EAN).HasMaxLength(13);
        builder.Property(x => x.CodigoBarras).HasMaxLength(50);
        builder.Property(x => x.UrlFoto).HasMaxLength(512);
        builder.Property(x => x.TipoArtigo).HasConversion<int>();

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => new { x.ClinicaId, x.NumeroArtigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => x.ClinicaId);

        builder.HasOne(x => x.UnidadeMedida)
            .WithMany()
            .HasForeignKey(x => x.UnidadeMedidaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FamiliaArtigo)
            .WithMany()
            .HasForeignKey(x => x.FamiliaArtigoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TaxaIva)
            .WithMany()
            .HasForeignKey(x => x.TaxaIvaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MotivoIsencao)
            .WithMany()
            .HasForeignKey(x => x.MotivoIsencaoId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Armazem)
            .WithMany()
            .HasForeignKey(x => x.ArmazemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}