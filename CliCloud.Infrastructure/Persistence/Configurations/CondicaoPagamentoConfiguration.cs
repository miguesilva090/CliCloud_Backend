using CliCloud.Domain.Entities.Pagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class CondicaoPagamentoConfiguration : IEntityTypeConfiguration<CondicaoPagamento>
{
    public void Configure(EntityTypeBuilder<CondicaoPagamento> builder)
    {
        builder.ToTable("CondicaoPagamento", "Pagamentos");

        builder.Property(x => x.Descricao).HasMaxLength(30);
        builder.Property(x => x.Desconto).HasColumnType("decimal(18,2)");

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => x.ClinicaId);
    }
}
