using CliCloud.Domain.Entities.Sinistros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class EstadoSinistroItemConfiguration : IEntityTypeConfiguration<EstadoSinistroItem>
    {
        public void Configure(EntityTypeBuilder<EstadoSinistroItem> builder)
        {
            builder.ToTable("EstadoSinistro", "Sinistros");
            builder.HasIndex(x => x.Designacao);
        }
    }
}