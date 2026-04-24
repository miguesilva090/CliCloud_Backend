using Ardalis.Specification;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Specifications;

public class NotificacaoTipoMatchDesignacaoTipo : Specification<NotificacaoTipo>
{
  public NotificacaoTipoMatchDesignacaoTipo(string designacaoTipo)
  {
    _ = Query.Where(x => x.DesignacaoTipo == designacaoTipo);
  }
}
