using Ardalis.Specification;
using CliCloud.Domain.Entities.GruposSanguineos;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.Specifications
{
    public class GrupoSanguineoSearchList : Specification<GrupoSanguineo>
    {
        public GrupoSanguineoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
