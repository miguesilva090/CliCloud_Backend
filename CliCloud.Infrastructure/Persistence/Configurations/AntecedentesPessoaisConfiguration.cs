using CliCloud.Domain.Entities.Antecedentes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AntecedentesPessoaisConfiguration : IEntityTypeConfiguration<AntecedentesPessoais>
    {
        public void Configure(EntityTypeBuilder<AntecedentesPessoais> builder)
        {
            builder.ToTable("AntecedentesPessoais", "Antecedentes");
        }

    }
}