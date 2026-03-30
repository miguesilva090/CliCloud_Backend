using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class TipoDeDorConfiguration : IEntityTypeConfiguration<TipoDeDor>
    {
        public void Configure(EntityTypeBuilder<TipoDeDor> builder)
        {
            builder.ToTable("TiposDeDor", "Tratamentos");
        }
    }
}