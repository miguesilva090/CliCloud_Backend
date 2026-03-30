using Ardalis.Specification;
using CliCloud.Domain.Entities.Antecedentes;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Specifications
{
    public class AntecedentesFamiliaresUtenteMatchNameDoenca : Specification<AntecedentesFamiliaresUtente>
    {
        public AntecedentesFamiliaresUtenteMatchNameDoenca(string? nomeDoenca)
        {
            if (!string.IsNullOrWhiteSpace(nomeDoenca))
            {
                _ = Query.Where(h => h.NomeDoenca == nomeDoenca);
            }

            _ = Query.OrderBy(h => h.NomeDoenca);
        }
    }
}
