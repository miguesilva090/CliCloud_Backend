using CliCloud.Domain.Entities.Stocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class FamiliaArtigoConfiguration : IEntityTypeConfiguration<FamiliaArtigo>
{
    public void Configure(EntityTypeBuilder<FamiliaArtigo> builder)
    {
        builder.ToTable("FamiliaArtigo", "Stocks");

        builder.Property(x => x.Descricao).HasMaxLength(50);
        builder.Property(x => x.UrlFoto).HasMaxLength(512);

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => new { x.ClinicaId, x.ParentId });
        builder.HasIndex(x => x.ClinicaId);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}