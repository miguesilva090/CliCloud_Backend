using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class TipoLoteConfiguration : IEntityTypeConfiguration<TipoLote>
    {
        public void Configure(EntityTypeBuilder<TipoLote> builder)
        {
            _ = builder.ToTable("TipoLotes", "Credenciais");
            _ = builder.HasKey(x => x.Id);
            _ = builder.Property(x => x.Id).ValueGeneratedNever();
            _ = builder.Property(x => x.Designa).HasMaxLength(50);
        }
    }
}
