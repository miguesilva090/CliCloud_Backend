using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications
{
    public class LoteDirectSearchTable : Specification<LoteDirect>
    {
        public LoteDirectSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            Query.Include(x => x.Utente);

            foreach (var filter in filters ?? [])
            {
                switch((filter.Id ?? "").ToLowerInvariant())
                {
                    case "credencial":
                        if(!string.IsNullOrWhiteSpace(filter.Value))
                            Query.Where(x => x.Credencial != null && x.Credencial.Contains(filter.Value));
                        break;
                    case "numerolote":
                        if(int.TryParse(filter.Value, out var numeroLote))
                            Query.Where(x => x.NumeroLote == numeroLote);
                        break;
                    case "codigoorganismo":
                        if(int.TryParse(filter.Value, out var codigoOrganismo))
                            Query.Where(x => x.CodigoOrganismo == codigoOrganismo);
                        break;
                    case "ano":
                        if(int.TryParse(filter.Value, out var ano))
                            Query.Where(x => x.Ano == ano);
                        break;
                    case "mes":
                        if(int.TryParse(filter.Value, out var mes))
                            Query.Where(x => x.Mes == mes);
                        break;
                    case "historico":
                        if(bool.TryParse(filter.Value, out var historico))
                            Query.Where(x => x.Historico == historico);
                        break;
                    case "indicelote":
                        if(int.TryParse(filter.Value, out var indiceLote))
                            Query.Where(x => x.IndiceLote == indiceLote);
                        break;
                }
            }

            if(string.IsNullOrWhiteSpace(dynamicOrder))
                Query.OrderByDescending(x => x.CreatedOn);
            else 
                Query.OrderBy(dynamicOrder);
        }
    }
}