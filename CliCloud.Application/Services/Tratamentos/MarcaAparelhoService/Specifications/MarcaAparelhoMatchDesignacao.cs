using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Specifications
{
    public class MarcaAparelhoMatchDesignacao : Specification<MarcaAparelho>
    {
        public MarcaAparelhoMatchDesignacao(string? designacao)
        {
            if (!string.IsNullOrWhiteSpace(designacao))
            {
                _ = Query.Where(h => h.Designacao == designacao);
            }
            _ = Query.OrderBy(h => h.Designacao);
        }
    }
}
