using CliCloud.Domain.Entities.Notificacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
  public void Configure(EntityTypeBuilder<Notificacao> builder)
  {
    builder.ToTable("Notificacao", "Notificacoes");
    builder
      .HasOne(x => x.NotificacaoTipo)
      .WithMany()
      .HasForeignKey(x => x.NotificacaoTipoId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
