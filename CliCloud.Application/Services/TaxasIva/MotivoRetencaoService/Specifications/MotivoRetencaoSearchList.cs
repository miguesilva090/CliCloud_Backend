using Ardalis.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Specifications
{
    public class MotivoRetencaoSearchList : Specification<MotivoRetencao>
    {
        public MotivoRetencaoSearchList(string? keyword = "", string? tipoImposto = null)
        {
            if (!string.IsNullOrWhiteSpace(tipoImposto))
                _ = Query.Where(x => x.TipoImposto == tipoImposto);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Descricao.Contains(keyword) ||
                    x.Codigo.ToString().Contains(keyword) ||
                    x.TipoImposto.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Codigo);
        }
    }
}
