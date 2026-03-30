using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class MotivosDesmarcacaoConfiguration : IEntityTypeConfiguration<MotivosDesmarcacao>
    {
        public void Configure(EntityTypeBuilder<MotivosDesmarcacao> builder)
        {
            builder.ToTable("MotivosDesmarcacao", "Tratamentos");
        }
    }
}