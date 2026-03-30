using CliCloud.Domain.Entities.Alergias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AlergiaConfiguration : IEntityTypeConfiguration<Alergia>
    {
        public void Configure(EntityTypeBuilder<Alergia> builder)
        {
            builder.ToTable("Alergia", "Alergias");
        }
    }
}
