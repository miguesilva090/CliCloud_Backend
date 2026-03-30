using Ardalis.Specification;
using CliCloud.Domain.Entities.GrausParentesco;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Specifications
{
    public class GrauParentescoMatchDescricao : Specification<GrauParentesco>
    {
        public GrauParentescoMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
