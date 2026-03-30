using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class PrioridadeConfiguration : IEntityTypeConfiguration<Prioridade>
    {
        public void Configure(EntityTypeBuilder<Prioridade> builder)
        {
            builder.ToTable("Prioridades", "Tratamentos");
        }
    }
}
