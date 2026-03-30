using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class EstadoListaEsperaConfiguration : IEntityTypeConfiguration<EstadoListaEspera>
    {
        public void Configure(EntityTypeBuilder<EstadoListaEspera> builder)
        {
            builder.ToTable("EstadosListaEspera", "Tratamentos");
        }
    }
}
