using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.Specifications
{
    public class UtentePatologiaComparticipacaoByUtenteId : Specification<UtentePatologiaComparticipacao>
    {
        public UtentePatologiaComparticipacaoByUtenteId(Guid utenteId)
        {
            _ = Query
                .Where(x => x.UtenteId == utenteId)
                .OrderBy(x => x.CodigoComparticipacao);
        }
    }
}