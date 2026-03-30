using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class EvolucaoTratamentoConfiguration : IEntityTypeConfiguration<EvolucaoTratamento>
    {
        public void Configure(EntityTypeBuilder<EvolucaoTratamento> builder)
        {
            builder.ToTable("EvolucaoTratamento", "Tratamentos");
        }
    }
}