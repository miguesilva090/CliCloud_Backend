using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.GrauAlergiaService.Specifications
{
    public class GrauAlergiaMatchDescricao : Specification<GrauAlergia>
    {
        public GrauAlergiaMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
