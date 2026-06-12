using Ardalis.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Specifications
{
    public class ContaBancariaSearchList : Specification<ContaBancaria>
    {
        public ContaBancariaSearchList(string keyword = "")
        {
            _ = Query.Include(x => x.Banco);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string k = keyword.Trim();
                _ = Query.Where(x =>
                    x.Numero.Contains(k) ||
                    x.TipoConta.Contains(k) ||
                    (x.IBAN != null && x.IBAN.Contains(k)) ||
                    (x.Banco != null && x.Banco.Nome.Contains(k)));
            }

            _ = Query.OrderBy(x => x.Numero);
        }
    }
}