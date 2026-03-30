using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.Specifications
{
    public class HorarioTecnicoVariavelSearchByTecnicoId : Specification<HorarioTecnicoVariavel>
    {
        public HorarioTecnicoVariavelSearchByTecnicoId(Guid tecnicoId)
        {
            _ = Query.Where(x => x.TecnicoId == tecnicoId);
            _ = Query.Include(x => x.Tecnico);
            _ = Query.OrderBy(x => x.Data);
        }
    }
}