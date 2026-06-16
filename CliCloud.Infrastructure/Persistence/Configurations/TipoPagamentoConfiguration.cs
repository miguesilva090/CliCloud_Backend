using CliCloud.Domain.Entities.Pagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class TipoPagamentoConfiguration : IEntityTypeConfiguration<TipoPagamento>
{
    public void Configure(EntityTypeBuilder<TipoPagamento> builder)
    {
        builder.ToTable("TipoPagamento", "Pagamentos");

        builder.Property(x => x.Codigo).HasMaxLength(3);
        builder.Property(x => x.Descricao).HasMaxLength(50);

        builder.HasIndex(x => x.Codigo)
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");
    }
}
