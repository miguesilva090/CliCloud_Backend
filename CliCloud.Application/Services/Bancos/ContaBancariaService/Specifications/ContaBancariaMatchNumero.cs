using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Specifications
{
    public class ContaBancariaMatchNumero : Specification<ContaBancaria>
    {
        public ContaBancariaMatchNumero(string numero)
        {
            _ = Query.Where(x => x.Numero == numero);
        }
    }
}