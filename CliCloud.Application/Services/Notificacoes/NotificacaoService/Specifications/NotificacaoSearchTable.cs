using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;
using CliCloud.Domain.Entities.Notificacoes;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.Specifications;

public class NotificacaoSearchTable : Specification<Notificacao>
{
  public NotificacaoSearchTable(
    List<TableFilter> filters,
    string? dynamicOrder,
    NotificacaoListMode listMode,
    Guid utilizadorId,
    Guid? clinicaId)
  {
    _ = Query.Include(x => x.NotificacaoTipo);

    ApplyListMode(listMode, utilizadorId, clinicaId);

    if (filters is { Count: > 0 })
    {
      foreach (TableFilter filter in filters)
      {
        string id = (filter.Id ?? "").ToLowerInvariant();
        switch (id)
        {
          case "titulo":
            if (!string.IsNullOrWhiteSpace(filter.Value))
              _ = Query.Where(x => x.Titulo.Contains(filter.Value));
            break;
          case "estado":
            if (int.TryParse(filter.Value, out int estado))
              _ = Query.Where(x => x.Estado == estado);
            break;
          case "prioridade":
            if (int.TryParse(filter.Value, out int prioridade))
              _ = Query.Where(x => x.Prioridade == prioridade);
            break;
          case "notificacaotipoid":
            if (Guid.TryParse(filter.Value, out Guid tipoId))
              _ = Query.Where(x => x.NotificacaoTipoId == tipoId);
            break;
          default:
            break;
        }
      }
    }

    if (string.IsNullOrEmpty(dynamicOrder))
      _ = Query.OrderByDescending(x => x.CreatedOn);
    else
      _ = Query.OrderBy(dynamicOrder);
  }

  private void ApplyListMode(NotificacaoListMode listMode, Guid utilizadorId, Guid? clinicaId)
  {
    switch (listMode)
    {
      case NotificacaoListMode.Inbox:
        _ = Query.Where(x =>
          x.DestinatarioUtilizadorId == utilizadorId
          && (clinicaId == null
            ? x.ClinicaDestinoId == null
            : x.ClinicaDestinoId == null || x.ClinicaDestinoId == clinicaId));
        break;
      case NotificacaoListMode.Enviadas:
        _ = Query.Where(x =>
          x.RemetenteId == utilizadorId
          && (clinicaId == null
            ? x.ClinicaDestinoId == null
            : x.ClinicaDestinoId == null || x.ClinicaDestinoId == clinicaId));
        break;
      // Apenas avisos de atualização (estado «Atualização clínica»), não todos os anúncios à empresa.
      case NotificacaoListMode.AtualizacoesClinica:
        if (clinicaId is Guid cid)
          _ = Query.Where(x =>
            x.ClinicaDestinoId == cid
            && x.DestinatarioUtilizadorId == null
            && x.Estado == 3);
        else
          _ = Query.Where(_ => false);
        break;
      default:
        _ = Query.Where(_ => false);
        break;
    }
  }
}
