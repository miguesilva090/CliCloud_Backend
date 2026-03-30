using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class NotaBodyChartConfiguration : IEntityTypeConfiguration<NotaBodyChart>
    {
        public void Configure(EntityTypeBuilder<NotaBodyChart> builder)
        {
            builder.ToTable("NotaBodyChart", "ProcessoClinico");
        }
    }
}