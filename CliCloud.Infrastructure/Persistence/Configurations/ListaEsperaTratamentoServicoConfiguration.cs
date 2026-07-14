using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class ListaEsperaTratamentoServicoConfiguration
    : IEntityTypeConfiguration<ListaEsperaTratamentoServico>
{
    public void Configure(EntityTypeBuilder<ListaEsperaTratamentoServico> builder)
    {
        builder.ToTable("ListaEsperaTratamentoServico", "Tratamentos");

        builder.HasIndex(x => new { x.ListaEsperaTratamentoId, x.Ordem });

        builder.Property(x => x.CodigoServico).HasMaxLength(50);
        builder.Property(x => x.Designacao).HasMaxLength(250);
        builder.Property(x => x.SubsistemaDesignacao).HasMaxLength(250);
        builder.Property(x => x.Duracao).HasMaxLength(50);

        builder
            .HasOne(x => x.ListaEsperaTratamento)
            .WithMany(x => x.Servicos)
            .HasForeignKey(x => x.ListaEsperaTratamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Servico)
            .WithMany()
            .HasForeignKey(x => x.ServicoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
