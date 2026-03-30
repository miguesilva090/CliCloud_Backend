using Ardalis.Specification;
using CliCloud.Domain.Entities.Empresas;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Specifications
{
    public class EmpresaSearchList : Specification<Empresa>
    {
        public EmpresaSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}

