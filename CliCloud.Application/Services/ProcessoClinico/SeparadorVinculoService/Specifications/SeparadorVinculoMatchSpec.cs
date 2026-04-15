using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.Specifications;

public class SeparadorVinculoMatchSpec : Specification<SeparadorVinculo>
{
    public SeparadorVinculoMatchSpec(Guid separadorId, TipoVinculoSeparador tipo, Guid entidadeId)
    {
        _ = Query.Where(x => x.SeparadorId == separadorId && x.Tipo == tipo && x.EntidadeId == entidadeId);
    }
}
