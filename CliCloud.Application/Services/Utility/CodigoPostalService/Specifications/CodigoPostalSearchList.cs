using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;


namespace CliCloud.Application.Services.Utility.CodigoPostalService.Specifications
{
    public class CodigoPostalSearchList : Specification<CodigoPostal>
    {
        public CodigoPostalSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Codigo.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Codigo); // ordem alfabética pelo código

        }
    }
}
