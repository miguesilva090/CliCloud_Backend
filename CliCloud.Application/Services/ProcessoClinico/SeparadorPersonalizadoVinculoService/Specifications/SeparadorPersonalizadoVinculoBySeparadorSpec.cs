using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.Specifications;

public class SeparadorPersonalizadoVinculoBySeparadorSpec
    : Specification<SeparadorPersonalizadoVinculo>
{
    public SeparadorPersonalizadoVinculoBySeparadorSpec(Guid separadorPersonalizadoId)
    {
        _ = Query.Where(x => x.SeparadorPersonalizadoId == separadorPersonalizadoId);
    }
}
