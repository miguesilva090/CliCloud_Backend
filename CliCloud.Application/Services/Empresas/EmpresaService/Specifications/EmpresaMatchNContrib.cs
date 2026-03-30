using Ardalis.Specification;
using CliCloud.Domain.Entities.Empresas;

namespace CliCloud.Application.Services.Empresas.EmpresaService.Specifications
{
    public class EmpresaMatchNContrib : Specification<Empresa>
    {
        public EmpresaMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}

