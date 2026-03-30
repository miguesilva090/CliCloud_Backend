using Ardalis.Specification;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.Specifications
{
    public class CentroSaudeSearchList : Specification<CentroSaudeEntity>
    {
        public CentroSaudeSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
