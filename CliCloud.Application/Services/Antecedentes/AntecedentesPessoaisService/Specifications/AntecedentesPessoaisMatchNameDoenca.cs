using Ardalis.Specification;
using CliCloud.Domain.Entities.Antecedentes;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Specifications
{
    public class AntecedentesPessoaisMatchNameDoenca : Specification<AntecedentesPessoais>
    {
        public AntecedentesPessoaisMatchNameDoenca(string? nomeDoenca)
        {
            if (!string.IsNullOrWhiteSpace(nomeDoenca))
            {
                _ = Query.Where(h => h.NomeDoenca == nomeDoenca);
            }
            _ = Query.OrderBy(h => h.NomeDoenca);
        }
    }
}
