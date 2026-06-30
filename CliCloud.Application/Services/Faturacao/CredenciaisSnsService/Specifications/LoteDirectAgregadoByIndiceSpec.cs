using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Specifications;

public sealed class LoteDirectAgregadoByIndiceSpec : Specification<LoteDirectAgregado>
{
    public LoteDirectAgregadoByIndiceSpec(int indice)
    {
        Query.Where(x => x.Indice == indice);
    }
}