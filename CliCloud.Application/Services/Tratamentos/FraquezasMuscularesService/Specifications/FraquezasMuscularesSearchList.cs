using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Specifications
{
    public class FraquezasMuscularesSearchList : Specification<FraquezasMusculares>
    {
        public FraquezasMuscularesSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.CreatedOn); // default sort order

        }
    }
}
