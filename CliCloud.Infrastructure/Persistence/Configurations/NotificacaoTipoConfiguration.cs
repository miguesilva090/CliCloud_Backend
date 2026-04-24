using CliCloud.Domain.Entities.Notificacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliCloud.Infrastructure.Persistence.Configurations;

public class NotificacaoTipoConfiguration : IEntityTypeConfiguration<NotificacaoTipo>
{
  public void Configure(EntityTypeBuilder<NotificacaoTipo> builder)
  {
    builder.ToTable("NotificacaoTipo", "Notificacoes");
  }
}
