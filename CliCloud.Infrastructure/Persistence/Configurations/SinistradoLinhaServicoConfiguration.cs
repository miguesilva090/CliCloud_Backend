using CliCloud.Domain.Entities.Sinistros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class SinistradoLinhaServicoConfiguration : IEntityTypeConfiguration<SinistradoLinhaServico>
    {
        public void Configure(EntityTypeBuilder<SinistradoLinhaServico> builder)
        {
            builder.ToTable("SinistradoLinhaServico", "Sinistros");

            builder.HasIndex(x => x.SinistradoId);
            builder.HasIndex(x => x.CodigoServico);
            builder.HasIndex(x => x.ServicoId);
            builder.Property(x => x.CodigoServico).HasMaxLength(40);

            builder.HasOne(x => x.Sinistrado)
                .WithMany(x => x.LinhasServico)
                .HasForeignKey(x => x.SinistradoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}