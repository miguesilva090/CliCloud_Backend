using Ardalis.Specification;
using CliCloud.Domain.Entities.Empresas;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Specifications
{
  public class EmpresaByIdWithIncludes : Specification<Empresa>
  {
    public EmpresaByIdWithIncludes(Guid id)
    {
      _ = Query
        .Include(x => x.Rua)
        .Include(x => x.CodigoPostal)
        .Include(x => x.Freguesia)
        .Include(x => x.Concelho)
        .Include(x => x.Distrito)
        .Include(x => x.Pais)
        .Include(x => x.EntidadeContactos)
        .Where(x => x.Id == id);
    }
  }
}

