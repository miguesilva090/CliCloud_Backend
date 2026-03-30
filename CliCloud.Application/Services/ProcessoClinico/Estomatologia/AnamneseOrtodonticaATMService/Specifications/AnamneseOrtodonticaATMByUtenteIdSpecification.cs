using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Specifications
{
    public class AnamneseOrtodonticaATMByUtenteIdSpecification : Specification<AnamneseOrtodonticaATM>
    {
        public AnamneseOrtodonticaATMByUtenteIdSpecification(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
    }
}