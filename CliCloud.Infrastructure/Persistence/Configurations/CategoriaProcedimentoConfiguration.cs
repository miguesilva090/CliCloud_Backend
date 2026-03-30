using CliCloud.Domain.Entities.Exames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class CategoriaProcedimentoConfiguration : IEntityTypeConfiguration<CategoriaProcedimento>
    {
        public void Configure(EntityTypeBuilder<CategoriaProcedimento> builder)
        {
            builder.ToTable("CategoriaProcedimento", "Exames");
        }
    }
}
