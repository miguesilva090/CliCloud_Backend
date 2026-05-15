using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class LoteDirectLinhaConfiguration : IEntityTypeConfiguration<LoteDirectLinha>
{
    public void Configure(EntityTypeBuilder<LoteDirectLinha> builder)
    {
        builder.ToTable("LoteDirectLinha", "Credenciais");

        builder.HasIndex(x => x.LoteDirectId);
        builder.HasIndex(x => x.ServicoId);

        builder.HasOne(x => x.LoteDirect)
            .WithMany(x => x.Linhas)
            .HasForeignKey(x => x.LoteDirectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Servico)
            .WithMany()
            .HasForeignKey(x => x.ServicoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}