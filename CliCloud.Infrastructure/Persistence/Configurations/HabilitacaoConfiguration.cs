using CliCloud.Domain.Entities.Habilitacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class HabilitacaoConfiguration : IEntityTypeConfiguration<Habilitacao>
    {
        public void Configure(EntityTypeBuilder<Habilitacao> builder)
        {
            builder.ToTable("Habilitacao", "Utility");
        }
    }
}
