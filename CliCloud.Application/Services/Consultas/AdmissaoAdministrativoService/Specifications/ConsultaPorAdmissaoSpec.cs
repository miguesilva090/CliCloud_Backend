using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class ConsultaPorAdmissaoSpec : Specification<Consulta>
{
    public ConsultaPorAdmissaoSpec(Guid admissaoId)
    {
        _ = Query.Where(x => x.AdmissaoId == admissaoId && x.DeletedOn == null);
    }
}