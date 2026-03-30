using Ardalis.Specification;
using CliCloud.Domain.Entities.RegioesCorpo;


namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Specifications
{
    public class RegiaoCorpoSearchList : Specification<RegiaoCorpo>
    {
        public RegiaoCorpoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao); // default sort order

        }
    }
}
