using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public class TipoLoteByIdsSpec : Specification<TipoLote>
{
    public TipoLoteByIdsSpec(IReadOnlyCollection<int> ids)
    {
        Query.Where(x => ids.Contains(x.Id));
    }
}
