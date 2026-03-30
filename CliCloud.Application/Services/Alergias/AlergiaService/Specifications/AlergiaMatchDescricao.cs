using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.Alergias.AlergiaService.Specifications
{
    public class AlergiaMatchDescricao : Specification<Alergia>
    {
        public AlergiaMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
