using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissaoByConsultaIdSpec : Specification<Admissao>
{
    public AdmissaoByConsultaIdSpec(Guid consultaId)
    {
        _ = Query.Where(x => x.Consulta != null && x.Consulta.Id == consultaId && x.DeletedOn == null);
    }
}