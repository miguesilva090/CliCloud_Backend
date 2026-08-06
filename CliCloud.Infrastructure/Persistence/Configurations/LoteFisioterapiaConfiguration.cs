using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class LoteFisioterapiaConfiguration : IEntityTypeConfiguration<LoteFisioterapia>
{
    public void Configure(EntityTypeBuilder<LoteFisioterapia> builder)
    {
        builder.ToTable("LoteFisioterapia", "Credenciais");

        builder.HasIndex(x => x.Indice).IsUnique();
        builder.HasIndex(x => new { x.Ano, x.Mes });
        builder.HasIndex(x => new
        {
            x.Ano,
            x.Mes,
            x.CodigoOrganismo,
            x.TipoLote,
            x.TipoServico,
            x.NumeroLote,
        });
    }
}
