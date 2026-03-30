using CliCloud.Domain.Entities.ProvenienciasUtente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ProvenienciaUtenteConfiguration : IEntityTypeConfiguration<ProvenienciaUtente>
    {
        public void Configure(EntityTypeBuilder<ProvenienciaUtente> builder)
        {
            builder.ToTable("ProvenienciaUtente", "Utility");
        }
    }
}
