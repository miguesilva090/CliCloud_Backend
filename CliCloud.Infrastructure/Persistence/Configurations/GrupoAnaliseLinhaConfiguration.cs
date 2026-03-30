using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrupoAnaliseLinhaConfiguration : IEntityTypeConfiguration<GrupoAnaliseLinha>
    {
        public void Configure(EntityTypeBuilder<GrupoAnaliseLinha> builder)
        {
            builder.ToTable("GrupoAnaliseLinha", "Exames");
        }
    }
}
