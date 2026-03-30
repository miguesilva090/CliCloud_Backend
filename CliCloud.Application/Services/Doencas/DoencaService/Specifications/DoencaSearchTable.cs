using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Doencas;

namespace CliCloud.Application.Services.Doencas.DoencaService.Specifications
{
    public class DoencaSearchTable : Specification<Doenca>
    {
        public DoencaSearchTable(string? keyword = "", string? dynamicOrder = "", Guid? parentId = null)
        {
            if (parentId.HasValue)
            {
                _ = Query.Where(x => x.ParentId == parentId.Value);
            }
            else if (string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.ParentId == null);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => (x.Title != null && x.Title.Contains(keyword)) || (x.Code != null && x.Code.Contains(keyword)));
            }
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Code ?? x.Title);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}
