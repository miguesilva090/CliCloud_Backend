using CliCloud.Domain.Entities.Faturacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class AdsePreFaturaConfiguration : IEntityTypeConfiguration<AdsePreFatura>
{
    public void Configure(EntityTypeBuilder<AdsePreFatura> builder)
    {
        builder.ToTable("AdsePreFatura", "Faturacao");
        builder.HasIndex(x => new { x.ClinicaId, x.TipoPreFatura, x.NumOrdem })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");
        builder.HasOne(x => x.DocumentoFecho)
            .WithMany()
            .HasForeignKey(x => x.DocumentoFechoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
