using Ardalis.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.Specifications
{
    public class TaxaIvaMatchDescricao : Specification<TaxaIva>
    {
        public TaxaIvaMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
