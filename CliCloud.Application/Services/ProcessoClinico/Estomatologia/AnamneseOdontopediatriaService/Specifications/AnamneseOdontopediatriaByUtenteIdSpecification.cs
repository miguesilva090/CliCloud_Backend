using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Specifications
{
    public class AnamneseOdontopediatriaByUtenteIdSpecification : Specification<AnamneseOdontopediatria>
    {
        public AnamneseOdontopediatriaByUtenteIdSpecification(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId);
        }
    }
}

