using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.Specifications
{
    public class AnamneseOrtodonticaDenticaoDeciduaeMistaByUtenteIdSpecification : Specification<AnamneseOrtodonticaDenticaoDeciduaeMista>
    {
        public AnamneseOrtodonticaDenticaoDeciduaeMistaByUtenteIdSpecification(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
    }
}