using Ardalis.Specification;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Specifications;

public class NotificacaoTipoExistsById : Specification<NotificacaoTipo>
{
  public NotificacaoTipoExistsById(Guid id)
  {
    _ = Query.Where(x => x.Id == id);
  }
}
