using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectAgregadoMaxIndiceSpec : Specification<LoteDirectAgregado, int>
{
    public LoteDirectAgregadoMaxIndiceSpec()
    {
        Query.OrderByDescending(x => x.Indice);
        Query.Take(1);
        Query.Select(x => x.Indice);
    }
}
