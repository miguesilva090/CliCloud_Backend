using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.Alergias.AlergiaService.Specifications
{
    public class AlergiaSearchList : Specification<Alergia>
    {
        public AlergiaSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao != null && x.Descricao.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
