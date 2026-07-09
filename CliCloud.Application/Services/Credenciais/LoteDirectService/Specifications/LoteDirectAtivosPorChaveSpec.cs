using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public sealed class LoteDirectAtivosPorChaveSpec : Specification<LoteDirect>
{
    public LoteDirectAtivosPorChaveSpec(int codigoOrganismo, int mes, int ano, int tipoLote)
    {
        Query.Where(x =>
            !x.Historico
            && x.CodigoOrganismo == codigoOrganismo
            && x.Mes == mes
            && x.Ano == ano
            && x.TipoLote == tipoLote
            && x.NumeroLote.HasValue);
    }
}
