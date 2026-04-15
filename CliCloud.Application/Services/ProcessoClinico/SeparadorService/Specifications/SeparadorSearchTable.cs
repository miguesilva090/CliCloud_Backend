using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.Specifications;

public class SeparadorSearchTable : Specification<Separador>
{
    public SeparadorSearchTable(string? keyword = "", string? dynamicOrder = "")
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            _ = Query.Where(x => x.Nome.Contains(keyword));
        }

        if (string.IsNullOrEmpty(dynamicOrder))
        {
            _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome);
        }
        else
        {
            _ = Query.OrderBy(dynamicOrder);
        }
    }
}
