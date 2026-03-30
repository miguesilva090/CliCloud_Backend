using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService.Specifications
{
  public class TecnicoSearchByName : Specification<Tecnico>
  {
    public TecnicoSearchByName(string? name)
    {
      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.Freguesia)
        .ThenInclude(x => x.Concelho)
        .ThenInclude(x => x.Distrito);

        _ = Query.Include(x => x.Rua)
          .ThenInclude(x => x.CodigoPostal);

        _ = Query.Include(x => x.EntidadeContactos);

        _ = Query.Include(x => x.Especialidade);

        if(!string.IsNullOrWhiteSpace(name))
        {
          _ = Query.Where(x => x.Nome.Contains(name));
        }

        _ = Query.OrderBy(x => x.Nome);
    }
  }
}
