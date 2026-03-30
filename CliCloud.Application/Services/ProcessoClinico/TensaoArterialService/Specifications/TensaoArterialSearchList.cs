using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Specifications
{
    public class TensaoArterialSearchList : Specification<TensaoArterial>
    {
        public TensaoArterialSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.UtenteId.ToString().Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
