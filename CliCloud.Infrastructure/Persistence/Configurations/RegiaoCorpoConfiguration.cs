using CliCloud.Domain.Entities.RegioesCorpo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class RegiaoCorpoConfiguration : IEntityTypeConfiguration<RegiaoCorpo>
    {
        public void Configure(EntityTypeBuilder<RegiaoCorpo> builder)
        {
            builder.ToTable("RegiaoCorpo", "RegioesCorpo");
        }
    }
}
