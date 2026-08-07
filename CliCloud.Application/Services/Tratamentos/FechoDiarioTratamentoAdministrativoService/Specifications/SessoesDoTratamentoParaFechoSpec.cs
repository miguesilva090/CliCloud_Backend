using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.Specifications;

public sealed class SessoesDoTratamentoParaFechoSpec : Specification<SessaoTratamento>
{
    public SessoesDoTratamentoParaFechoSpec(Guid tratamentoId)
    {
        _ = Query
            .Where(x => x.DeletedOn == null)
            .Where(x => x.TratamentoId == tratamentoId)
            .Include(x => x.Servicos);
    }
}