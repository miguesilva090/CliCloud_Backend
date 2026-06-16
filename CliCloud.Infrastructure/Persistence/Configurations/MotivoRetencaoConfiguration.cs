using CliCloud.Domain.Entities.TaxasIva;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MotivoRetencaoConfiguration : IEntityTypeConfiguration<MotivoRetencao>
    {
        public void Configure(EntityTypeBuilder<MotivoRetencao> builder)
        {
            builder.ToTable("MotivoRetencao", "Utility");
            builder.Property(x => x.Descricao).HasMaxLength(150);
            builder.Property(x => x.TipoImposto).HasMaxLength(3);
            builder.HasIndex(x => x.Codigo).IsUnique();
            builder.HasIndex(x => x.TipoImposto);
        }
    }
}
