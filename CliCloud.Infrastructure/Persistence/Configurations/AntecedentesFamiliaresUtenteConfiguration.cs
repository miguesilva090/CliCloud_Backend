using CliCloud.Domain.Entities.Antecedentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AntecedentesFamiliaresUtenteConfiguration : IEntityTypeConfiguration<AntecedentesFamiliaresUtente>
    {
        public void Configure(EntityTypeBuilder<AntecedentesFamiliaresUtente> builder)
        {
            builder.ToTable("AntecedentesFamiliaresUtente", "Antecedentes");
        }
    }
}