using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectAgregadoPorChaveSpec : Specification<LoteDirectAgregado>
{
    public LoteDirectAgregadoPorChaveSpec(
        int codigoOrganismo,
        int tipoLote,
        int tipoServico,
        int mes, 
        int ano
    )
    {
        Query.Where(x => 
            x.CodigoOrganismo == codigoOrganismo &&
            x.TipoLote == tipoLote &&
            x.TipoServico == tipoServico &&
            x.Mes == mes &&
            x.Ano == ano)
        .OrderBy(x => x.NumeroLote);
    }
}