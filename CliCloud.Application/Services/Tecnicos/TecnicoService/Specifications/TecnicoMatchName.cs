using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService.Specifications
{
    public class TecnicoMatchName : Specification<Tecnico>
    {
        public TecnicoMatchName(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _ = Query.Where(h => h.Nome == name);
            }
            _ = Query.OrderBy(h => h.Nome);
        }
    }
}
