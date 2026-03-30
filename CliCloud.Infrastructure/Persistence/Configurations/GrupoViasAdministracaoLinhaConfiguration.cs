using CliCloud.Domain.Entities.Artigos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrupoViasAdministracaoLinhaConfiguration : IEntityTypeConfiguration<GrupoViasAdministracaoLinha>
    {
        public void Configure(EntityTypeBuilder<GrupoViasAdministracaoLinha> builder)
        {
            builder.ToTable("GrupoViasAdministracaoLinha", "Artigos");

            builder.Property(x => x.Linha)
                .IsRequired();

            builder.HasOne(x => x.Grupo)
                .WithMany(g => g.Vias)
                .HasForeignKey(x => x.GrupoId);

            builder.HasOne(x => x.Via)
                .WithMany(v => v.Grupos)
                .HasForeignKey(x => x.ViaId);
        }
    }
}

