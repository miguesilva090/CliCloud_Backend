using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MotivoAltaConfiguration : IEntityTypeConfiguration<MotivoAlta>
    {
        public void Configure(EntityTypeBuilder<MotivoAlta> builder)
        {
            builder.ToTable("MotivoAlta", "Tratamentos");
        }
    }
}
