using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.Specifications
{
    public class UtentePatologiaComparticipacaoByUtenteAndCodigo : Specification<UtentePatologiaComparticipacao>
    {
        public UtentePatologiaComparticipacaoByUtenteAndCodigo(Guid utenteId, int codigoComparticipacao)
        {
            _ = Query.Where(x =>
                x.UtenteId == utenteId &&
                x.CodigoComparticipacao == codigoComparticipacao);
        }
    }
}