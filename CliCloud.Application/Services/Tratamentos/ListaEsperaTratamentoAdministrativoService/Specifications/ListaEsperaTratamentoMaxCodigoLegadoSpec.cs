using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;

public sealed class ListaEsperaTratamentoMaxCodigoLegadoSpec : Specification<ListaEsperaTratamento>
{
    public ListaEsperaTratamentoMaxCodigoLegadoSpec()
    {
        _ = Query
            .Where(x => x.CodigoLegado != null)
            .OrderByDescending(x => x.CodigoLegado)
            .Take(1);
    }
}
