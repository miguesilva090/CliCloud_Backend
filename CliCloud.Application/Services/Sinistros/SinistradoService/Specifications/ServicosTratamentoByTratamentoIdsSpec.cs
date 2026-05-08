using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ServicosTratamentoByTratamentoIdsSpec : Specification<ServicoTratamento>
    {
        public ServicosTratamentoByTratamentoIdsSpec(List<Guid> tratamentoIds)
        {
            _ = Query
                .Where(x => tratamentoIds.Contains(x.TratamentoId))
                .Include(x => x.Servico);
        }
    }
}
