using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MapaBodyChartConfiguration : IEntityTypeConfiguration<MapaBodyChart>
    {
        public void Configure(EntityTypeBuilder<MapaBodyChart> builder)
        {
            builder.ToTable("MapaBodyChart", "ProcessoClinico");
        }
    }
}