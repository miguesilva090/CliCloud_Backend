using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Specifications;

public sealed class AdmissoesOrdemEntradaPorMedicoDataSpec : Specification<Admissao>
{
  public AdmissoesOrdemEntradaPorMedicoDataSpec(Guid medicoId, DateTime data)
  {
    _ = Query.Where(x =>
      x.DeletedOn == null
      && x.MedicoId == medicoId
      && x.Data.HasValue
      && x.Data.Value.Date == data.Date
    );
  }
}
