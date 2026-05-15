using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class LoteDirectDetalheConfiguration : IEntityTypeConfiguration<LoteDirectDetalhe>
{
    public void Configure(EntityTypeBuilder<LoteDirectDetalhe> builder)
    {
        builder.ToTable("LoteDirectDetalhe", "Credenciais");

        builder.HasIndex(x => new { x.Ano, x.Mes });
        builder.HasIndex(x => x.LoteDirectId);

        builder.HasOne(x => x.LoteDirectAgregado)
            .WithMany()
            .HasForeignKey(x => x.LoteDirectAgregadoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LoteDirect)
            .WithMany()
            .HasForeignKey(x => x.LoteDirectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}