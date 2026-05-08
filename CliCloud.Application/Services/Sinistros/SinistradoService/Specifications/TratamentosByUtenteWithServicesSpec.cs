using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class TratamentosByUtenteWithServicesSpec : Specification<Tratamento>
    {
        public TratamentosByUtenteWithServicesSpec(Guid utenteId)
        {
            _ = Query
                .Where(x => x.UtenteId == utenteId && (x.Faturado ?? 0) == 0)
                .Include(x => x.Servicos)
                    .ThenInclude(x => x.Servico)
                .OrderByDescending(x => x.DataInic ?? x.Data ?? x.CreatedOn);
        }
    }
}
