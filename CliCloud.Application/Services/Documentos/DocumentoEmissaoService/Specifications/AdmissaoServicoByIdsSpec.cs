using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class AdmissaoServicoByIdsSpec : Specification<AdmissaoServico>
{
    public AdmissaoServicoByIdsSpec(IEnumerable<Guid> ids)
    {
        var list = ids.Distinct().ToList();
        _ = Query.Where(x => list.Contains(x.Id));
    }
}