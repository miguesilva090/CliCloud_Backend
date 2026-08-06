using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class RequisicaoEspLinhaConfiguration : IEntityTypeConfiguration<RequisicaoEspLinha>
{
    public void Configure(EntityTypeBuilder<RequisicaoEspLinha> builder)
    {
        builder.ToTable("RequisicaoEspLinha", "Consultas");

        builder.HasIndex(x => x.Codigo).IsUnique();
        builder.HasIndex(x => x.RequisicaoEspId);
    }
}
