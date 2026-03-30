using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.Specifications
{
  public class TratamentoSearchList : Specification<Tratamento>
  {
    public TratamentoSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.UtenteId == g
            || x.MedicoId == g
            || x.FisioterapeutaId == g
            || x.AuxiliarId == g
            || x.OutroTecnicoId == g
            || x.OrganismoId == g
            || x.ReciboId == g
            || x.SeguradoraId == g
            || x.DocumentoId == g
            || x.TratamentoPredId == g
            || x.LocalTratamentoId == g
            || x.LocalOrigemId == g
            || x.SinistroId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.Designacao != null && x.Designacao.Contains(keyword))
            || (x.Obs != null && x.Obs.Contains(keyword))
            || (x.TecObs != null && x.TecObs.Contains(keyword))
            || (x.Credencial != null && x.Credencial.Contains(keyword))
            || (x.NumDevolucao != null && x.NumDevolucao.Contains(keyword))
            || (x.NumDestacavel != null && x.NumDestacavel.Contains(keyword))
            || (x.NumCartao != null && x.NumCartao.Contains(keyword))
            || (x.NumBenif != null && x.NumBenif.Contains(keyword))
            || (x.Apolice != null && x.Apolice.Contains(keyword))
            || (x.NomePatologia != null && x.NomePatologia.Contains(keyword))
          );
        }
      }

      _ = Query.OrderByDescending(x => x.DataInic ?? x.CreatedOn);
    }
  }
}

