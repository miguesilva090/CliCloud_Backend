using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissaoServicosPorAdmissaoIdSpec : Specification<AdmissaoServico>
{
  public AdmissaoServicosPorAdmissaoIdSpec(Guid admissaoId)
  {
    _ = Query.Where(s => s.AdmissaoId == admissaoId && s.DeletedOn == null);
  }
}
