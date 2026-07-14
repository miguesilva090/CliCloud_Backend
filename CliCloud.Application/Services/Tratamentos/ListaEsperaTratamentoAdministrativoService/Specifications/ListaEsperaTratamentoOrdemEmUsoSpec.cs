using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;

public sealed class ListaEsperaTratamentoOrdemEmUsoSpec : Specification<ListaEsperaTratamento>
{
    public ListaEsperaTratamentoOrdemEmUsoSpec(int ordem, Guid? excludeId = null)
    {
        _ = Query.Where(x =>
            x.DeletedOn == null
            && !x.Historico
            && x.Ordem == ordem
            && (excludeId == null || x.Id != excludeId)
        );
    }
}
