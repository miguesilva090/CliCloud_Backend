using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.Specifications
{
    public class TipoDeDorMatchDescricao : Specification<TipoDeDor>
    {
        public TipoDeDorMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
