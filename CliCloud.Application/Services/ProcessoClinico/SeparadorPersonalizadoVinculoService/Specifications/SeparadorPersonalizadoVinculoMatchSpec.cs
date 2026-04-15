using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.Specifications;

public class SeparadorPersonalizadoVinculoMatchSpec : Specification<SeparadorPersonalizadoVinculo>
{
    public SeparadorPersonalizadoVinculoMatchSpec(
        Guid separadorPersonalizadoId,
        TipoVinculoSeparador tipo,
        Guid entidadeId
    )
    {
        _ = Query.Where(x =>
            x.SeparadorPersonalizadoId == separadorPersonalizadoId
            && x.Tipo == tipo
            && x.EntidadeId == entidadeId
        );
    }
}
