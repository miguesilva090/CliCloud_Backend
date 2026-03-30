using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Specifications
{
    public class AnamneseOrtodonticaAnaliseDentariaByUtenteIdSpecification : Specification<AnamneseOrtodonticaAnaliseDentaria>
    {
        public AnamneseOrtodonticaAnaliseDentariaByUtenteIdSpecification(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
    }
}