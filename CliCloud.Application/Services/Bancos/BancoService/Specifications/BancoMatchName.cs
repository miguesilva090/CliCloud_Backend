using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.BancoService.Specifications
{
    public class BancoMatchName : Specification<Banco>
    {
        public BancoMatchName(string nome)
        {
            _ = Query.Where(x => x.Nome == nome);
        }
    }
}
