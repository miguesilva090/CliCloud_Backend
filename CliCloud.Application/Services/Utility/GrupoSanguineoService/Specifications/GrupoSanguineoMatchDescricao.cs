using Ardalis.Specification;
using CliCloud.Domain.Entities.GruposSanguineos;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.Specifications
{
    public class GrupoSanguineoMatchDescricao : Specification<GrupoSanguineo>
    {
        public GrupoSanguineoMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
