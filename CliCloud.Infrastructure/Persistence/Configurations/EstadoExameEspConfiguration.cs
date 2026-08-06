using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class EstadoExameEspConfiguration : IEntityTypeConfiguration<EstadoExameEsp>
{
    public void Configure(EntityTypeBuilder<EstadoExameEsp> builder)
    {
        builder.ToTable("EstadoExameEsp", "Consultas");

        builder.HasIndex(x => x.Codigo).IsUnique();
        builder.HasIndex(x => x.Abreviatura).IsUnique();
    }
}
