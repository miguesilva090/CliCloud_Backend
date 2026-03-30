using CliCloud.Domain.Entities.TaxasIva;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class TaxaIvaConfiguration : IEntityTypeConfiguration<TaxaIva>
    {
        public void Configure(EntityTypeBuilder<TaxaIva> builder)
        {
            builder.ToTable("TaxaIva", "Utility");
        }
    }
}
