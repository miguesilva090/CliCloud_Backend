using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class PeriocidadeTratamentoConfiguration : IEntityTypeConfiguration<PeriocidadeTratamento>
    {
        public void Configure(EntityTypeBuilder<PeriocidadeTratamento> builder)
        {
            builder.ToTable("PeriocidadeTratamento", "Tratamentos");
        }
    }
}