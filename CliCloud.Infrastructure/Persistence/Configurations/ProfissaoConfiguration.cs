using CliCloud.Domain.Entities.Profissoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class ProfissaoConfiguration : IEntityTypeConfiguration<Profissao>
    {
        public void Configure(EntityTypeBuilder<Profissao> builder)
        {
            builder.ToTable("Profissao", "Utility");
        }
    }
}
