using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectLinhasByCabecalhoSpec : Specification<LoteDirectLinha>
{
    public LoteDirectLinhasByCabecalhoSpec(Guid loteDirectId)
    {
        Query.Where(x => x.LoteDirectId == loteDirectId);
    }
}