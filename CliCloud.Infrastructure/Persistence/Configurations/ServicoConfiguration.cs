using CliCloud.Domain.Entities.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations
{
  public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
  {
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
      builder.ToTable("Servico", "Servicos");

      // Relacionamento N:1 com TipoServico (obrigatório)
      builder.HasOne(s => s.TipoServico)
        .WithMany(t => t.Servicos)
        .HasForeignKey(s => s.TipoServicoId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(s => s.TipoAparelho)
        .WithMany()
        .HasForeignKey(s => s.TipoAparelhoId)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(s => s.MotivoIsencao)
        .WithMany()
        .HasForeignKey(s => s.MotivoIsencaoId)
        .OnDelete(DeleteBehavior.Restrict);

      // Índice na Designacao para pesquisas (não único)
      builder.HasIndex(s => s.Designacao);
    }
  }
}
