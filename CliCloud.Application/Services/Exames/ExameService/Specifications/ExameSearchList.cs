using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.ExameService.Specifications
{
  public class ExameSearchList : Specification<Exame>
  {
    public ExameSearchList(string? keyword = "", Guid? utenteId = null)
    {
      if (utenteId.HasValue)
        _ = Query.Where(x => x.UtenteId == utenteId.Value);
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x =>
          (x.NumeroPrescricao != null && x.NumeroPrescricao.Contains(keyword)) ||
          (x.Observacoes != null && x.Observacoes.Contains(keyword)));
      _ = Query.OrderByDescending(x => x.DataPrescricao);
    }
  }
}
