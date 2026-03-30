using Ardalis.Specification;
using TipoEntidadeFinanceiraEntity = CliCloud.Domain.Entities.TipoEntidadeFinanceira.TipoEntidadeFinanceira;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Specifications
{
    public class TipoEntidadeFinanceiraSearchList : Specification<TipoEntidadeFinanceiraEntity>
    {
        public TipoEntidadeFinanceiraSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Designacao.Contains(keyword) || x.Sigla.Contains(keyword) || x.Dominio.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Designacao); // default sort order
        }
    }
}
