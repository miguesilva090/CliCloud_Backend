using CliCloud.Domain.Entities.Faturacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ReferenciaMbConfiguration : IEntityTypeConfiguration<ReferenciaMB>
{
    public void Configure(EntityTypeBuilder<ReferenciaMB> builder)
    {
        _ = builder.Property(x => x.ClienteNome).HasMaxLength(250);
        _ = builder.Property(x => x.Descricao).HasMaxLength(500);
        _ = builder.Property(x => x.Mensagem).HasMaxLength(2000);
        _ = builder.Property(x => x.EntidadeMb).HasMaxLength(20);
        _ = builder.Property(x => x.ReferenciaCodigo).HasMaxLength(50);
        _ = builder.Property(x => x.RequestId).HasMaxLength(120);

        _ = builder.Property(x => x.Valor).HasPrecision(18, 2);

        _ = builder.HasIndex(x => new { x.ClinicaId, x.CreatedOn });
        _ = builder.HasIndex(x => new { x.EntidadeMb, x.ReferenciaCodigo, x.RequestId });
    }
}