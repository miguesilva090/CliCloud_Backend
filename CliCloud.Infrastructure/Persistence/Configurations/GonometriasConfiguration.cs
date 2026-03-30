using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GoniometriasConfiguration : IEntityTypeConfiguration<Goniometrias>
    {
        public void Configure(EntityTypeBuilder<Goniometrias> builder)
        {
            builder.ToTable("Goniometrias", "Tratamentos");
        }
    }
}