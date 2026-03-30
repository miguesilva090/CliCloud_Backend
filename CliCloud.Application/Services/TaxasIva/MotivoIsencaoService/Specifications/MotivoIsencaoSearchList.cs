using Ardalis.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Specifications
{
    public class MotivoIsencaoSearchList : Specification<MotivoIsencao>
    {
        public MotivoIsencaoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Codigo.Contains(keyword) || x.Descricao.Contains(keyword));
            _ = Query.OrderBy(x => x.Codigo);
        }
    }
}
