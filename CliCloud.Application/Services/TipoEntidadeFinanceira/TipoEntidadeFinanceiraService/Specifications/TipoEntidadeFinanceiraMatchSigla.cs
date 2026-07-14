using Ardalis.Specification;
using TipoEntidadeFinanceiraEntity = CliCloud.Domain.Entities.TipoEntidadeFinanceira.TipoEntidadeFinanceira;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Specifications
{
    public class TipoEntidadeFinanceiraMatchSigla : Specification<TipoEntidadeFinanceiraEntity>
    {
        public TipoEntidadeFinanceiraMatchSigla(string sigla)
        {
            _ = Query.Where(x => x.Sigla == sigla);
        }
    }
}
