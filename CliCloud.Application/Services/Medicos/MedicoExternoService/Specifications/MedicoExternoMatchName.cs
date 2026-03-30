using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MedicoExternoService.Specifications
{
    public class MedicoExternoMatchName : Specification<MedicoExterno>
    {
        public MedicoExternoMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
