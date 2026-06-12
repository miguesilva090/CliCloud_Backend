using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class NaturezaDocumentoConfiguration : IEntityTypeConfiguration<NaturezaDocumento>
    {
        public void Configure(EntityTypeBuilder<NaturezaDocumento> builder)
        {
            builder.ToTable("NaturezaDocumento", "Documentos");

            builder.HasIndex(x => x.Sigla)
                .IsUnique();
        }
    }
}
