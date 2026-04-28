using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.Specifications
{
    public class HistoriaDentariaPorUtenteOrderedSpec : Specification<HistoriaDentaria>
    {
        public HistoriaDentariaPorUtenteOrderedSpec(Guid utenteId)
        {
            Query.Where(x => x.UtenteId == utenteId).OrderBy(x => x.DataRegisto);
        }
    }
}
