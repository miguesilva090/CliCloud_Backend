using Ardalis.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.Specifications
{
    public class TaxaIvaSearchList : Specification<TaxaIva>
    {
        public TaxaIvaSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Taxa).ThenBy( x => x.Descricao);
        }
    }
}
