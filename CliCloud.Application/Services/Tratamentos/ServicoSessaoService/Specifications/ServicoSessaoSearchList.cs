using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Specifications
{
  public class ServicoSessaoSearchList : Specification<ServicoSessao>
  {
    public ServicoSessaoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.SessaoTratamentoId == g
            || x.FisioterapeutaId == g
            || x.AuxiliarId == g
            || x.ServicoId == g
            || x.AparelhoId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.Obs != null && x.Obs.Contains(keyword))
            || (x.HoraInic != null && x.HoraInic.Contains(keyword))
            || (x.HoraFim != null && x.HoraFim.Contains(keyword))
            || (x.Duracao != null && x.Duracao.Contains(keyword))
          );
        }
      }
      _ = Query.OrderBy(x => x.Ordem ?? 0).ThenByDescending(x => x.CreatedOn);
    }
  }
}

