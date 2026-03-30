using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Specifications
{
    public class ModeloAparelhoMatchDesignacao : Specification<ModeloAparelho>
    {
        public ModeloAparelhoMatchDesignacao(string? designacao)
        {
            if (!string.IsNullOrWhiteSpace(designacao))
            {
                _ = Query.Where(h => h.Designacao == designacao);
            }
            _ = Query.OrderBy(h => h.Designacao);
        }
    }
}
