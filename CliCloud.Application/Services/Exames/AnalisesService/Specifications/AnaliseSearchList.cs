using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.AnalisesService.Specifications
{
    public class AnaliseSearchList : Specification<Analises>
    {
        public AnaliseSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x =>
                    (x.Nome != null && x.Nome.Contains(keyword)) ||
                    (x.UnidadeMedida != null && x.UnidadeMedida.Contains(keyword)) ||
                    (x.ValoresReferencia != null && x.ValoresReferencia.Contains(keyword)));
            _ = Query.OrderBy(x => x.Nome);
        }
    }
}
