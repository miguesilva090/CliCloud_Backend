using Ardalis.Specification;
using CliCloud.Domain.Entities.Doencas;

namespace CliCloud.Application.Services.Doencas.DoencaService.Specifications
{
    public class DoencaSearchList : Specification<Doenca>
    {
        public DoencaSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => (x.Title != null && x.Title.Contains(keyword)) || (x.Code != null && x.Code.Contains(keyword)));
            }
            _ = Query.OrderBy(x => x.Title);
        }
    }
}
