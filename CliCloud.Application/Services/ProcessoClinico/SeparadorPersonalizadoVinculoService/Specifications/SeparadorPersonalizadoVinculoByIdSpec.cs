using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.Specifications;

public class SeparadorPersonalizadoVinculoByIdSpec : Specification<SeparadorPersonalizadoVinculo>
{
    public SeparadorPersonalizadoVinculoByIdSpec(Guid id)
    {
        _ = Query.Where(x => x.Id == id);
    }
}
