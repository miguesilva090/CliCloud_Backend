using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.Specifications
{
    public class AlergiasUtenteObsByUtenteId : Specification<AlergiasUtenteObs>
    {
        public AlergiasUtenteObsByUtenteId(Guid utenteId)
        {
            _ = Query.Where(h => h.UtenteId == utenteId);
        }
    }
}
