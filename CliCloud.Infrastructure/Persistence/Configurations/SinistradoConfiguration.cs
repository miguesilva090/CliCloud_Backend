using CliCloud.Domain.Entities.Sinistros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class SinistradoConfiguration : IEntityTypeConfiguration<Sinistrado>
    {
        public void Configure(EntityTypeBuilder<Sinistrado> builder)
        {
            builder.ToTable("Sinistrado", "Sinistros");

            builder.HasIndex(x => x.CodigoSinistro).IsUnique();
            builder.HasIndex(x => x.Historico);
            builder.HasIndex(x => x.UtenteId);

            builder.HasOne(x => x.EstadoSinistro)
                .WithMany()
                .HasForeignKey(x => x.EstadoSinistroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}