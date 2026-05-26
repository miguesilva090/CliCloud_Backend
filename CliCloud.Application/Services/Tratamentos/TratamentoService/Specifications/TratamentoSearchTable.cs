using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.Specifications
{
  public class TratamentoSearchTable : Specification<Tratamento>
  {
    public TratamentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
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
            case "designacao":
              _ = Query.Where(x => x.Designacao != null && x.Designacao.Contains(val));
              break;
            case "utenteid":
              if (Guid.TryParse(val, out var utId)) _ = Query.Where(x => x.UtenteId == utId);
              break;
            case "medicoid":
              if (Guid.TryParse(val, out var mId)) _ = Query.Where(x => x.MedicoId == mId);
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
            case "organismoid":
              if (Guid.TryParse(val, out var oId)) _ = Query.Where(x => x.OrganismoId == oId);
              break;
            case "localtratamentoid":
              if (Guid.TryParse(val, out var ltId)) _ = Query.Where(x => x.LocalTratamentoId == ltId);
              break;
            case "tratamentopredid":
              if (Guid.TryParse(val, out var tpId)) _ = Query.Where(x => x.TratamentoPredId == tpId);
              break;
            case "localorigemid":
              if (Guid.TryParse(val, out var loId)) _ = Query.Where(x => x.LocalOrigemId == loId);
              break;
            case "reciboid":
              if (Guid.TryParse(val, out var rId)) _ = Query.Where(x => x.ReciboId == rId);
              break;
            case "seguradoraid":
              if (Guid.TryParse(val, out var sId)) _ = Query.Where(x => x.SeguradoraId == sId);
              break;
            case "documentoid":
              if (Guid.TryParse(val, out var dId)) _ = Query.Where(x => x.DocumentoId == dId);
              break;
            case "numcartao":
              _ = Query.Where(x => x.NumCartao != null && x.NumCartao.Contains(val));
              break;
            case "credencial":
              _ = Query.Where(x => x.Credencial != null && x.Credencial.Contains(val));
              break;
            case "apolice":
              _ = Query.Where(x => x.Apolice != null && x.Apolice.Contains(val));
              break;
            case "numbenif":
              _ = Query.Where(x => x.NumBenif != null && x.NumBenif.Contains(val));
              break;
            case "nomepatologia":
              _ = Query.Where(x => x.NomePatologia != null && x.NomePatologia.Contains(val));
              break;
            case "pago":
              if (int.TryParse(val, out var pago)) _ = Query.Where(x => x.Pago == pago);
              break;
            case "faturado":
              if (int.TryParse(val, out var fat)) _ = Query.Where(x => x.Faturado == fat);
              break;
            case "suspenso":
              if (int.TryParse(val, out var sus)) _ = Query.Where(x => x.Suspenso == sus);
              break;
            case "estadou":
              if (int.TryParse(val, out var eu)) _ = Query.Where(x => x.EstadoU == eu);
              break;
            case "estadoi":
              if (int.TryParse(val, out var ei)) _ = Query.Where(x => x.EstadoI == ei);
              break;
            case "provisorio":
              if (int.TryParse(val, out var prov)) _ = Query.Where(x => x.Provisorio == prov);
              break;
            case "isencao":
              if (int.TryParse(val, out var isenc)) _ = Query.Where(x => x.Isencao == isenc);
              break;
            case "datainic":
              if (DateTime.TryParse(val, out var di)) _ = Query.Where(x => x.DataInic.HasValue && x.DataInic.Value.Date == di.Date);
              break;
            case "datafim":
              if (DateTime.TryParse(val, out var df)) _ = Query.Where(x => x.DataFim.HasValue && x.DataFim.Value.Date == df.Date);
              break;
            case "data":
              if (DateTime.TryParse(val, out var d)) _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == d.Date);
              break;
          }
        }

      _ = Query
        .Include(x => x.Organismo)
        .Include(x => x.LocalTratamento)
        .Include(x => x.Medico)
        .Include(x => x.Sessoes)
        .Include(x => x.Servicos);

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderByDescending(x => x.DataInic ?? x.CreatedOn);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}

