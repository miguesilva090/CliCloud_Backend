using Ardalis.Specification;
using CliCloud.Domain.Entities.EstadosCivis;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Specifications
{
    public class EstadoCivilSearchList : Specification<EstadoCivil>
    {
        public EstadoCivilSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
