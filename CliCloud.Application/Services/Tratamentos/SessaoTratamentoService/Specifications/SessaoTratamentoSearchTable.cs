using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications
{
  public class SessaoTratamentoSearchTable : Specification<SessaoTratamento>
  {
    public SessaoTratamentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
      if (filters != null && filters.Count > 0)
        foreach (var f in filters)
        {
          var id = f.Id?.ToLowerInvariant();
          var val = f.Value;
          if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(val)) continue;

          switch (id)
          {
            case "id":
              if (Guid.TryParse(val, out var idGuid)) _ = Query.Where(x => x.Id == idGuid);
              break;
            case "tratamentoid":
              if (Guid.TryParse(val, out var tId)) _ = Query.Where(x => x.TratamentoId == tId);
              break;
            case "data":
              if (DateTime.TryParse(val, out var dt)) _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == dt.Date);
              break;
            case "numsessao":
              if (int.TryParse(val, out var ns)) _ = Query.Where(x => x.NumSessao == ns);
              break;
            case "horainic":
              _ = Query.Where(x => x.HoraInic != null && x.HoraInic.Contains(val));
              break;
            case "duracao":
              _ = Query.Where(x => x.Duracao != null && x.Duracao.Contains(val));
              break;
            case "fisioterapeutaid":
              if (Guid.TryParse(val, out var fisId)) _ = Query.Where(x => x.FisioterapeutaId == fisId);
              break;
            case "auxiliarid":
              if (Guid.TryParse(val, out var auxId)) _ = Query.Where(x => x.AuxiliarId == auxId);
              break;
            case "outrotecnicoid":
              if (Guid.TryParse(val, out var otId)) _ = Query.Where(x => x.OutroTecnicoId == otId);
              break;
            case "reciboid":
              if (Guid.TryParse(val, out var rId)) _ = Query.Where(x => x.ReciboId == rId);
              break;
            case "tipodocumentoid":
              if (Guid.TryParse(val, out var tdId)) _ = Query.Where(x => x.TipoDocumentoId == tdId);
              break;
            case "documentoid":
              if (Guid.TryParse(val, out var docId)) _ = Query.Where(x => x.DocumentoId == docId);
              break;
            case "pago":
              if (int.TryParse(val, out var pago)) _ = Query.Where(x => x.Pago == pago);
              break;
            case "faturado":
              if (int.TryParse(val, out var fat)) _ = Query.Where(x => x.Faturado == fat);
              break;
            case "faltou":
              if (int.TryParse(val, out var falt)) _ = Query.Where(x => x.Faltou == falt);
              break;
            case "compensafalta":
              if (int.TryParse(val, out var cf)) _ = Query.Where(x => x.CompensaFalta == cf);
              break;
            case "desmarcado":
              if (int.TryParse(val, out var desm)) _ = Query.Where(x => x.Desmarcado == desm);
              break;
            case "estadou":
              if (int.TryParse(val, out var eu)) _ = Query.Where(x => x.EstadoU == eu);
              break;
            case "estadoi":
              if (int.TryParse(val, out var ei)) _ = Query.Where(x => x.EstadoI == ei);
              break;
            case "obs":
              _ = Query.Where(x => x.Obs != null && x.Obs.Contains(val));
              break;
            case "observsessao":
              _ = Query.Where(x => x.ObservSessao != null && x.ObservSessao.Contains(val));
              break;
            case "obsfalta":
              _ = Query.Where(x => x.ObsFalta != null && x.ObsFalta.Contains(val));
              break;
          }
        }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

