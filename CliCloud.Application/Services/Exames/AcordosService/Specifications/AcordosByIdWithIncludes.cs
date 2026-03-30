using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.AcordosService.Specifications
{
    public class AcordosByIdWithIncludes : Specification<Acordos>
    {
        public AcordosByIdWithIncludes(Guid id)
        {
            _ = Query.Where(x => x.Id == id)
                .Include(x => x.TipoExame)
                .Include(x => x.Organismo);
        }
    }
}
