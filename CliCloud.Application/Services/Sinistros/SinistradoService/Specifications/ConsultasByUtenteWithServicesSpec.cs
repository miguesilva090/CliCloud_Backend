using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ConsultasByUtenteWithServicesSpec : Specification<Consulta>
    {
        public ConsultasByUtenteWithServicesSpec(Guid utenteId)
        {
            _ = Query
                .Where(x => x.UtenteId == utenteId)
                .Include(x => x.TipoConsultaItem)
                .Include(x => x.Servicos)
                    .ThenInclude(x => x.Servico)
                .OrderByDescending(x => x.Data ?? x.CreatedOn);
        }
    }
}
