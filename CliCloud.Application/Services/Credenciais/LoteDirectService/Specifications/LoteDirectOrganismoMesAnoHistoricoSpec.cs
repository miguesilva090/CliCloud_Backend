using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectOrganismoMesAnoHistoricoSpec : Specification<LoteDirect>
{
    public LoteDirectOrganismoMesAnoHistoricoSpec(int codigoOrganismo, int mes, int ano)
    {
        Query.Where(x =>
            x.CodigoOrganismo == codigoOrganismo
            && x.Mes == mes
            && x.Ano == ano
            && x.Historico);
    }
}
