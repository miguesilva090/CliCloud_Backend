using CliCloud.Domain.Entities.Faturacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class AdseCoPagamentoConfiguration : IEntityTypeConfiguration<AdseCoPagamento>
{
    public void Configure(EntityTypeBuilder<AdseCoPagamento> builder)
    {
        builder.ToTable("AdseCoPagamento", "Faturacao");
        builder.HasIndex(x => x.DocumentoId)
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");
        builder.HasOne(x => x.Documento)
            .WithMany()
            .HasForeignKey(x => x.DocumentoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
