using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Specifications;

public sealed class LoteDirectByIndiceLoteInSpec : Specification<LoteDirect>
{
    public LoteDirectByIndiceLoteInSpec(IReadOnlyCollection<int> indices)
    {
        if (indices is not { Count: > 0 })
        {
            Query.Where(_ => false);
            return;
        }

        int[] valores = indices.Distinct().ToArray();
        Query.Where(x => x.IndiceLote.HasValue && EF.Constant(valores).Contains(x.IndiceLote.Value));
        Query.Include(x => x.TipoServicoRegisto);
    }
}
