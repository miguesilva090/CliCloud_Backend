using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.Specifications;

public class SeparadorSearchList : Specification<Separador>
{
    public SeparadorSearchList(string? keyword = "")
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            _ = Query.Where(x => x.Nome.Contains(keyword));
        }

        _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome);
    }
}
