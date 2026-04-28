using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.Specifications
{
    public class MedicoPorIdUtilizadorSpec : Specification<Medico>
    {
        public MedicoPorIdUtilizadorSpec(Guid utilizadorId)
        {
            Query.Where(m => m.IdUtilizador == utilizadorId);
        }
    }
}
