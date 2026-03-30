using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.Specifications
{
    public class FolgasTecnicoSearchByTecnicoId : Specification<FolgasTecnico>
    {
        public FolgasTecnicoSearchByTecnicoId(Guid tecnicoId)
        {
            _ = Query.Where(x => x.TecnicoId == tecnicoId);
            _ = Query.Include(x => x.Tecnico);
            _ = Query.OrderBy(x => x.DataDe).ThenBy(x => x.DataAte);
        }
    }
}