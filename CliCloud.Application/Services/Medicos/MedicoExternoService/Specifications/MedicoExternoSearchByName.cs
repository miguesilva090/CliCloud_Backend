using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MedicoExternoService.Specifications
{
  public class MedicoExternoSearchByName : Specification<MedicoExterno>
  {
    public MedicoExternoSearchByName(string? name)
    {
      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.Freguesia)
        .ThenInclude(x => x.Concelho)
        .ThenInclude(x => x.Distrito);

        _ = Query.Include(x => x.Rua)
          .ThenInclude(x => x.CodigoPostal);

        _ = Query.Include(x => x.EntidadeContactos);

        if(!string.IsNullOrWhiteSpace(name))
        {
          _ = Query.Where(x => x.Nome.Contains(name));
        }

        _ = Query.OrderBy(x => x.Nome);
    }
  }
}
