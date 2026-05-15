using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Sinistros;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class SinistradoSearchTable : Specification<Sinistrado>
    {
        public SinistradoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            Query.Include(x => x.EstadoSinistro)
                 .Include(x => x.Utente);

            foreach( var filter in filters ?? [])
            {
                switch((filter.Id ?? "").ToLowerInvariant())
                {
                    case "codigosinistro":
                        if(!string.IsNullOrWhiteSpace(filter.Value))
                            Query.Where(x => x.CodigoSinistro.Contains(filter.Value));
                        break;
                    case "utenteid":
                        if(Guid.TryParse(filter.Value, out var utenteId))
                            Query.Where(x => x.UtenteId == utenteId);
                        break;
                    case "historico": 
                        if(bool.TryParse(filter.Value, out var historico))
                            Query.Where(x => x.Historico == historico);
                        break;
                }
            }

            if(string.IsNullOrEmpty(dynamicOrder))
                Query.OrderByDescending(x => x.CreatedOn);
            else 
                Query.OrderBy(dynamicOrder);
        }
    }
}