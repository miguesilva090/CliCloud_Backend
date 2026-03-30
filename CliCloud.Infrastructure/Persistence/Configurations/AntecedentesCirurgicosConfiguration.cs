using CliCloud.Domain.Entities.Antecedentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AntecedentesCirurgicosConfiguration : IEntityTypeConfiguration<AntecedentesCirurgicos>
    {
        public void Configure(EntityTypeBuilder<AntecedentesCirurgicos> builder)
        {
            builder.ToTable("AntecedentesCirurgicos", "Antecedentes");
        }
    }
}

