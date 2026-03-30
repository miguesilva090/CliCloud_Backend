using CliCloud.Domain.Entities.GrausParentesco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrauParentescoConfiguration : IEntityTypeConfiguration<GrauParentesco>
    {
        public void Configure(EntityTypeBuilder<GrauParentesco> builder)
        {
            builder.ToTable("GrauParentesco", "Utility");
        }
    }
}
