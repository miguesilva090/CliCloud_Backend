using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.Specifications
{
    public class MargemMedicoSearchByMedicoId : Specification<MargemMedico>
    {
        public MargemMedicoSearchByMedicoId(Guid medicoId)
        {
            _ = Query
                .Include(x => x.Servico)
                .Include(x => x.Medico)
                .Where(x => x.MedicoId == medicoId)
                .OrderByDescending(x => x.CreatedOn);
        }
    }
}
