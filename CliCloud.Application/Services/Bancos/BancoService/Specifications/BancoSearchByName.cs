using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.BancoService.Specifications
{
    public class BancoSearchByName : Specification<Banco>
    {
        public BancoSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
        }
    }
}
