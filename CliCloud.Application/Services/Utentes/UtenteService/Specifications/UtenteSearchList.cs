using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
    public class UtenteSearchList : Specification<Utente>
    {
        public UtenteSearchList(string? keyword = "")
        {
            _ = Query.Where(x => x.TipoEntidade == EntidadeTipo.Utente);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
