using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;

public sealed class ListaEsperaTratamentoMaxOrdemSpec : Specification<ListaEsperaTratamento>
{
    public ListaEsperaTratamentoMaxOrdemSpec()
    {
        _ = Query
            .Where(x => x.DeletedOn == null && !x.Historico)
            .OrderByDescending(x => x.Ordem)
            .Take(1);
    }
}
