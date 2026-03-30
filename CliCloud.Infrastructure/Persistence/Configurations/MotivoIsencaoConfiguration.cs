using CliCloud.Domain.Entities.TaxasIva;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MotivoIsencaoConfiguration : IEntityTypeConfiguration<MotivoIsencao>
    {
        public void Configure(EntityTypeBuilder<MotivoIsencao> builder)
        {
            builder.ToTable("MotivoIsencao", "Utility");
        }
    }
}
