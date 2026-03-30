using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;


namespace CliCloud.Application.Services.Utility.ConcelhoService.Specifications
{
    public class ConcelhoSearchList : Specification<Concelho>
    {
        public ConcelhoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Nome); // ordem alfabética

        }
    }
}
