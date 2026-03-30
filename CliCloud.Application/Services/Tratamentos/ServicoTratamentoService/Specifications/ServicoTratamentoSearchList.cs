using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Specifications
{
  public class ServicoTratamentoSearchList : Specification<ServicoTratamento>
  {
    public ServicoTratamentoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.TratamentoId == g
            || x.ServicoId == g
            || x.SessaoTratamentoId == g
          );
        }
        else
        {
          _ = Query.Where(x => x.Obs != null && x.Obs.Contains(keyword));
        }
      }
      _ = Query.OrderBy(x => x.Ordem ?? 0).ThenByDescending(x => x.CreatedOn);
    }
  }
}

