using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class RequisicaoEspEfetuadoNaoPrescritoConfiguration : IEntityTypeConfiguration<RequisicaoEspEfetuadoNaoPrescrito>
{
    public void Configure(EntityTypeBuilder<RequisicaoEspEfetuadoNaoPrescrito> builder)
    {
        builder.ToTable("RequisicaoEspEfetuadoNaoPrescrito", "Consultas");

        builder.HasIndex(x => x.Codigo).IsUnique();
        builder.HasIndex(x => x.RequisicaoEspId);
    }
}
