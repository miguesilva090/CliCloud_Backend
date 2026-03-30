using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;


namespace CliCloud.Application.Services.Utility.FreguesiaService.Specifications
{
    public class FreguesiaSearchList : Specification<Freguesia>
    {
        public FreguesiaSearchList(string? keyword = "")
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
