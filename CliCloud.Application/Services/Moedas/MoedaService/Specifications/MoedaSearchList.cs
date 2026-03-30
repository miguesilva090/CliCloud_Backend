using Ardalis.Specification;
using CliCloud.Domain.Entities.Moedas;

namespace CliCloud.Application.Services.Moedas.MoedaService.Specifications
{
    public class MoedaSearchList : Specification<Moeda>
    {
        public MoedaSearchList(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string k = keyword.Trim();
                _ = Query.Where(x =>
                    (x.Descricao != null && x.Descricao.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Abreviatura != null && x.Abreviatura.Contains(k, StringComparison.OrdinalIgnoreCase)));
            }
            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
