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

            builder.Property(x => x.CodigoSaft).HasMaxLength(12);
            builder.Property(x => x.Descricao).HasMaxLength(254);
            builder.Property(x => x.Norma).HasMaxLength(254);
            builder.Property(x => x.Mencao).HasMaxLength(254);

            builder.HasIndex(x => x.CodigoSaft);
        }
    }
}
