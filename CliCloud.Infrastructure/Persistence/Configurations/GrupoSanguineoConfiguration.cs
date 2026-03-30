using CliCloud.Domain.Entities.GruposSanguineos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrupoSanguineoConfiguration : IEntityTypeConfiguration<GrupoSanguineo>
    {
        public void Configure(EntityTypeBuilder<GrupoSanguineo> builder)
        {
            builder.ToTable("GrupoSanguineo", "Utility");
        }
    }
}
