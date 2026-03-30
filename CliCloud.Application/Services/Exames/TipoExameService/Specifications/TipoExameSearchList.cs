using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.TipoExameService.Specifications
{
    public class TipoExameSearchList : Specification<TipoExame>
    {
        public TipoExameSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => (x.Designacao != null && x.Designacao.Contains(keyword)) || (x.EAN != null && x.EAN.Contains(keyword)));
            _ = Query.OrderBy(x => x.Designacao);
        }
    }
}
