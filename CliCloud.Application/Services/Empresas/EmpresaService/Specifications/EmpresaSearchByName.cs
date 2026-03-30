using Ardalis.Specification;
using CliCloud.Domain.Entities.Empresas;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Specifications
{
    public class EmpresaSearchByName : Specification<Empresa>
    {
        public EmpresaSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
        }
    }
}

