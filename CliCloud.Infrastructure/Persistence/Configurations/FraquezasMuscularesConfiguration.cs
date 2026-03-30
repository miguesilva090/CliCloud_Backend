using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class FraquezasMuscularesConfiguration : IEntityTypeConfiguration<FraquezasMusculares>
    {
        public void Configure(EntityTypeBuilder<FraquezasMusculares> builder)
        {
            builder.ToTable("FraquezasMusculares", "Tratamentos");
        }
    }
}
