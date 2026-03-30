using CliCloud.Domain.Entities.Artigos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class GrupoViasAdministracaoConfiguration : IEntityTypeConfiguration<GrupoViasAdministracao>
    {
        public void Configure(EntityTypeBuilder<GrupoViasAdministracao> builder)
        {
            builder.ToTable("GrupoViasAdministracao", "Artigos");

            builder.Property(x => x.Descricao)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}

