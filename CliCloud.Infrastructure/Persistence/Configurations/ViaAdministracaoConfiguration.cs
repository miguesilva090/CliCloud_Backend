using CliCloud.Domain.Entities.Artigos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ViaAdministracaoConfiguration : IEntityTypeConfiguration<ViaAdministracao>
    {
        public void Configure(EntityTypeBuilder<ViaAdministracao> builder)
        {
            builder.ToTable("ViaAdministracao", "Artigos");

            builder.Property(x => x.Descricao)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}

