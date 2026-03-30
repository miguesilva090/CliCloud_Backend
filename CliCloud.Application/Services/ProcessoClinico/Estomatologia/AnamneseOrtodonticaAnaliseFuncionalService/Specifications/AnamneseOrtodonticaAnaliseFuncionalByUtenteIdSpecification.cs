using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Specifications
{
    public class AnamneseOrtodonticaAnaliseFuncionalByUtenteIdSpecification : Specification<AnamneseOrtodonticaAnaliseFuncional>
    {
        public AnamneseOrtodonticaAnaliseFuncionalByUtenteIdSpecification(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
    }
}