using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Specifications 
{
    public class AnamneseOrtodonticaByUtenteIdSpecification : Specification<AnamneseOrtodonticaAnaliseGeral>
     {
        public AnamneseOrtodonticaByUtenteIdSpecification(Guid utenteId) 
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
     }
}