using Ardalis.Specification;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Specifications;

public class NotificacaoByIdWithTipo : Specification<Notificacao>
{
  public NotificacaoByIdWithTipo()
  {
    _ = Query.Include(x => x.NotificacaoTipo);
  }
}
