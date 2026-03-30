using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ModeloRelatorioAtestadoConfiguration : IEntityTypeConfiguration<ModeloRelatorioAtestado>
    {
        public void Configure(EntityTypeBuilder<ModeloRelatorioAtestado> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TextoHtml)
                .IsRequired();
        }
    }
}

