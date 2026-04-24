using Ardalis.Specification;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Specifications;

public class NotificacaoTipoSearchList : Specification<NotificacaoTipo>
{
  public NotificacaoTipoSearchList(string? keyword = "")
  {
    if (!string.IsNullOrWhiteSpace(keyword))
      _ = Query.Where(x => x.DesignacaoTipo.Contains(keyword));

    _ = Query.OrderBy(x => x.DesignacaoTipo);
  }
}
