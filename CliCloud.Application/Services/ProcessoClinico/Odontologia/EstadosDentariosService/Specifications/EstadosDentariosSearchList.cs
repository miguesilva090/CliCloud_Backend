using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Specifications
{
    public class EstadosDentariosSearchList : Specification<EstadosDentarios>
    {
        public EstadosDentariosSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                Query.Where(x =>
                    x.Codigo.Contains(keyword) ||
                    x.Descricao.Contains(keyword));
            }

            Query.OrderBy(x => x.Codigo);
        }
    }
}
