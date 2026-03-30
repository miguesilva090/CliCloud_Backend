using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications
{
  public class SessaoTratamentoSearchList : Specification<SessaoTratamento>
  {
    public SessaoTratamentoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.TratamentoId == g
            || x.FisioterapeutaId == g
            || x.AuxiliarId == g
            || x.OutroTecnicoId == g
            || x.ReciboId == g
            || x.TipoDocumentoId == g
            || x.DocumentoId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.Obs != null && x.Obs.Contains(keyword))
            || (x.ObservSessao != null && x.ObservSessao.Contains(keyword))
            || (x.ObsFalta != null && x.ObsFalta.Contains(keyword))
            || (x.NumTransacao != null && x.NumTransacao.Contains(keyword))
            || (x.NumDevolucao != null && x.NumDevolucao.Contains(keyword))
            || (x.NumDestacavel != null && x.NumDestacavel.Contains(keyword))
          );
        }
      }

      _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
    }
  }
}

