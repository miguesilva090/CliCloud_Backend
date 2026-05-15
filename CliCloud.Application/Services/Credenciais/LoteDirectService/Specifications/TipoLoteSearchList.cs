using Ardalis.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications
{
    public class TipoLoteSearchList : Specification<TipoLote>
    {
        public TipoLoteSearchList()
        {
            _ = Query.OrderBy(x => x.Designa);
        }
    }
}
