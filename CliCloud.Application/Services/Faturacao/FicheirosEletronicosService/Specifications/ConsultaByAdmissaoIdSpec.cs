using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class ConsultaByAdmissaoIdSpec : Specification<Consulta>
{
    public ConsultaByAdmissaoIdSpec(Guid admissaoId)
    {
        _ = Query.Where(x => x.AdmissaoId == admissaoId && x.DeletedOn == null);
    }
}