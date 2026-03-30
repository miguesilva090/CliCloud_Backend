using Ardalis.Specification;
using CliCloud.Domain.Entities.Fornecedores;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.Specifications
{
  public class FornecedorByIdWithIncludes : Specification<Fornecedor>
  {
    public FornecedorByIdWithIncludes(Guid id)
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
