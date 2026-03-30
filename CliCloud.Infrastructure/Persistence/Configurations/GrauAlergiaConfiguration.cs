using CliCloud.Domain.Entities.Alergias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrauAlergiaConfiguration : IEntityTypeConfiguration<GrauAlergia>
    {
        public void Configure(EntityTypeBuilder<GrauAlergia> builder)
        {
            builder.ToTable("GrauAlergia", "Alergias");
        }
    }
}
