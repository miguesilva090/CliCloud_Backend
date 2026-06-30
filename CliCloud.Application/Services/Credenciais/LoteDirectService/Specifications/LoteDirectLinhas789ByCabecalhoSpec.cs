using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectLinhas789ByCabecalhoSpec : Specification<LoteDirectLinha789>
{
    public LoteDirectLinhas789ByCabecalhoSpec(Guid loteDirectId)
    {
        Query
            .Where(x => x.LoteDirectId == loteDirectId)
            .Include(x => x.Servico)
            .OrderBy(x => x.CreatedOn);
    }
}