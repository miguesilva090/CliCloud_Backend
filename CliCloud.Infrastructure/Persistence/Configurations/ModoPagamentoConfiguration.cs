using CliCloud.Domain.Entities.Pagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ModoPagamentoConfiguration : IEntityTypeConfiguration<ModoPagamento>
{
    public void Configure(EntityTypeBuilder<ModoPagamento> builder)
    {
        builder.ToTable("ModoPagamento", "Pagamentos");

        builder.Property(x => x.Descricao).HasMaxLength(50);
        builder.Property(x => x.Abreviatura).HasMaxLength(3);

        builder.HasIndex(x => new { x.ClinicaId, x.Codigo })
            .IsUnique()
            .HasFilter("[DeletedOn] IS NULL");

        builder.HasIndex(x => x.ClinicaId);
        builder.HasIndex(x => x.Abreviatura);

        builder.HasOne(x => x.TipoPagamento)
            .WithMany()
            .HasForeignKey(x => x.Abreviatura)
            .HasPrincipalKey(t => t.Codigo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ContaBancaria)
            .WithMany()
            .HasForeignKey(x => x.ContaBancariaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
