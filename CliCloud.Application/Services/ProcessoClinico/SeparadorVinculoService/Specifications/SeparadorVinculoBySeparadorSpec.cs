using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.Specifications;

public class SeparadorVinculoBySeparadorSpec : Specification<SeparadorVinculo>
{
    public SeparadorVinculoBySeparadorSpec(Guid separadorId)
    {
        _ = Query.Where(x => x.SeparadorId == separadorId);
    }
}
