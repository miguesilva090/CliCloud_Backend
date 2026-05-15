using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class LoteDirectAgregadoConfiguration : IEntityTypeConfiguration<LoteDirectAgregado>
{
    public void Configure(EntityTypeBuilder<LoteDirectAgregado> builder)
    {
        builder.ToTable("LoteDirectAgregado", "Credenciais");

        builder.Property(x => x.Indice)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => new { x.Ano, x.Mes });
        builder.HasIndex(x => new {
            x.Ano,
            x.Mes,
            x.CodigoOrganismo,
            x.TipoLote,
            x.TipoServico,
            x.NumeroLote,
        }).IsUnique();
    }
}