using Ardalis.Specification;
using CliCloud.Domain.Entities.GrausParentesco;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Specifications
{
    public class GrauParentescoSearchList : Specification<GrauParentesco>
    {
        public GrauParentescoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
