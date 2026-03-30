using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class TipoExameConfiguration : IEntityTypeConfiguration<TipoExame>
    {
        public void Configure(EntityTypeBuilder<TipoExame> builder)
        {
            builder.ToTable("TipoExame", "Exames");
        }
    }
}
