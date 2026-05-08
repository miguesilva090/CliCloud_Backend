using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ServicosTratamentoByTratamentoIdSpec : Specification<ServicoTratamento>
    {
        public ServicosTratamentoByTratamentoIdSpec(Guid tratamentoId)
        {
            _ = Query
                .Where(x => x.TratamentoId == tratamentoId)
                .Include(x => x.Servico);
        }
    }
}
