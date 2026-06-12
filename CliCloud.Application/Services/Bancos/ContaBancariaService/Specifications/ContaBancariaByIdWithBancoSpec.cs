using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Specifications
{
    public class ContaBancariaByIdWithBancoSpec : Specification<ContaBancaria>
    {
        public ContaBancariaByIdWithBancoSpec(Guid id)
        {
            _ = Query.Where(x => x.Id == id).Include(x => x.Banco);
        }
    }
}