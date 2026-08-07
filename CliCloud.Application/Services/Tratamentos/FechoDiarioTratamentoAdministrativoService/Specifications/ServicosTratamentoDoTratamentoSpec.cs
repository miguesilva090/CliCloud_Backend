using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.Specifications;

public sealed class ServicosTratamentoDoTratamentoSpec : Specification<ServicoTratamento>
{
    public ServicosTratamentoDoTratamentoSpec(Guid tratamentoId)
    {
        _ = Query
            .Where(x => x.DeletedOn == null)
            .Where(x => x.TratamentoId == tratamentoId);
    }
}