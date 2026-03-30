using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class TipoConsultaItemConfiguration : IEntityTypeConfiguration<TipoConsultaItem>
    {
        public void Configure(EntityTypeBuilder<TipoConsultaItem> builder)
        {
            builder.ToTable("TiposConsulta", "Consultas");
        }
    }
}
