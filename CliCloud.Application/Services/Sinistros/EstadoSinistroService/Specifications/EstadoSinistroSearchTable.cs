using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Sinistros;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService.Specifications
{
    public class EstadoSinistroSearchTable : Specification<EstadoSinistroItem>
    {
        public EstadoSinistroSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            foreach (var filter in filters ?? [] )
            {
                if((filter.Id ?? "").ToLowerInvariant() == "designacao" && !string.IsNullOrWhiteSpace(filter.Value))
                    Query.Where(x => x.Designacao.Contains(filter.Value));
            }

            if(string.IsNullOrEmpty(dynamicOrder))
                Query.OrderBy(x => x.Designacao);
            else
                Query.OrderBy(dynamicOrder);
        }
    }
}