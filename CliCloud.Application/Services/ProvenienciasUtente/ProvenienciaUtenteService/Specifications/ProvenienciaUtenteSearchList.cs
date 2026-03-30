using Ardalis.Specification;
using CliCloud.Domain.Entities.ProvenienciasUtente;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Specifications
{
    public class ProvenienciaUtenteSearchList : Specification<ProvenienciaUtente>
    {
        public ProvenienciaUtenteSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
