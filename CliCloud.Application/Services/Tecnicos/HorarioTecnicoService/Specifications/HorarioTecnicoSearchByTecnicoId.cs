using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Specifications
{
    public class HorarioTecnicoSearchByTecnicoId : Specification<HorarioTecnico>
    {
        public HorarioTecnicoSearchByTecnicoId(Guid tecnicoId)
        {
            _ = Query.Where(x => x.TecnicoId == tecnicoId);
            _ = Query.Include(x => x.Tecnico)
                .Include(x => x.Horarios);
            _ = Query.OrderBy(x => x.CreatedOn);
        }
    }
}
