using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Bancos;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Specifications
{
    public class ContaBancariaSearchTable : Specification<ContaBancaria>
    {
        public ContaBancariaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query.Include(x => x.Banco);

            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "numero":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Numero.Contains(filter.Value));
                            break;
                        case "tipoconta":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.TipoConta.Contains(filter.Value));
                            break;
                        case "banconome":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Banco != null && x.Banco.Nome.Contains(filter.Value));
                            break;
                        case "iban":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.IBAN != null && x.IBAN.Contains(filter.Value));
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Numero);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}