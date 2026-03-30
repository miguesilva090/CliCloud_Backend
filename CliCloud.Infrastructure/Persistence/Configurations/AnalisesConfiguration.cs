using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AnalisesConfiguration : IEntityTypeConfiguration<Analises>
    {
        public void Configure(EntityTypeBuilder<Analises> builder)
        {
            builder.ToTable("Analises", "Exames");
        }
    }
}
