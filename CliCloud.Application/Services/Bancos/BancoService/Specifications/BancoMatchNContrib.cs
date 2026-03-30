using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.BancoService.Specifications
{
    public class BancoMatchNContrib : Specification<Banco>
    {
        public BancoMatchNContrib(string ncontrib)
        {
            _ = Query.Where(x => x.NumeroContribuinte != null && x.NumeroContribuinte == ncontrib);
        }
    }
}
