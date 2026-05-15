using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
    public class AdmissaoServicoConfiguration : IEntityTypeConfiguration<AdmissaoServico>
    {
        public void Configure(EntityTypeBuilder<AdmissaoServico> builder)
        {
            builder.ToTable("AdmissaoServico", "Consultas");

            builder.HasOne(s => s.Admissao)
                .WithMany(a => a.Servicos)
                .HasForeignKey(s => s.AdmissaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Servico)
                .WithMany()
                .HasForeignKey(s => s.ServicoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Exame)
                .WithMany()
                .HasForeignKey(s => s.ExameId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(s => new { s.AdmissaoId, s.Linha}).IsUnique();
        }
    }
}