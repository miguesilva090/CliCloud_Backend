using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class RequisicaoEspConfiguration : IEntityTypeConfiguration<RequisicaoEsp>
{
    public void Configure(EntityTypeBuilder<RequisicaoEsp> builder)
    {
        builder.ToTable("RequisicaoEsp", "Consultas");

        builder.HasIndex(x => x.Codigo).IsUnique();
        builder.HasIndex(x => x.NumeroRequisicao);

        builder.HasMany(x => x.Linhas)
            .WithOne(x => x.RequisicaoEsp)
            .HasForeignKey(x => x.RequisicaoEspId);

        builder.HasMany(x => x.EfetuadosNaoPrescritos)
            .WithOne(x => x.RequisicaoEsp)
            .HasForeignKey(x => x.RequisicaoEspId);
    }
}
