using CliCloud.Domain.Entities.Stocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class SubsistemaArtigoConfiguration : IEntityTypeConfiguration<SubsistemaArtigo>
{
    public void Configure(EntityTypeBuilder<SubsistemaArtigo> builder)
    {
        builder.ToTable("SubsistemaArtigo", "Stocks");

        builder.Property(x => x.CodigoCartaoInstituicao).HasMaxLength(20);
        builder.Property(x => x.CodigoComplementarAdse).HasMaxLength(10);
        builder.Property(x => x.ValorServico).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MargemOrganismoPercent).HasColumnType("decimal(18,2)");
        builder.Property(x => x.ValorOrganismo).HasColumnType("decimal(18,2)");
        builder.Property(x => x.ValorUtente).HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.ClinicaId);
        builder.HasIndex(x => x.CodigoCartaoInstituicao);

        builder.HasIndex(x => new { x.ClinicaId, x.ArtigoId, x.OrganismoId }).IsUnique().HasFilter("[DeletedOn] IS NULL");

        builder.HasOne(x => x.Artigo).WithMany().HasForeignKey(x => x.ArtigoId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Organismo).WithMany().HasForeignKey(x => x.OrganismoId).OnDelete(DeleteBehavior.Restrict);
    }
}