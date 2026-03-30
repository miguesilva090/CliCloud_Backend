using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class LocalTratamentoConfiguration : IEntityTypeConfiguration<LocalTratamento>
    {
        public void Configure(EntityTypeBuilder<LocalTratamento> builder)
        {
            builder.ToTable("LocaisTratamento", "Tratamentos");
        }
    }
}
