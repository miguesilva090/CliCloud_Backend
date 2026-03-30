using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;


namespace CliCloud.Application.Services.GrauAlergiaService.Specifications
{
    public class GrauAlergiaSearchList : Specification<GrauAlergia>
    {
        public GrauAlergiaSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao);

        }
    }
}
