using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

public class LoteDirectAgregadoSearchTable : Specification<LoteDirectAgregado>
{
    public LoteDirectAgregadoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
    {
        foreach (TableFilter filter in filters ?? [])
        {
            switch ((filter.Id ?? "").ToLowerInvariant())
            {
                case "ano":
                    if (int.TryParse(filter.Value, out int ano))
                        Query.Where(x => x.Ano == ano);
                    break;
                case "mes":
                    if (int.TryParse(filter.Value, out int mes))
                        Query.Where(x => x.Mes == mes);
                    break;
                case "numerolote":
                    if (int.TryParse(filter.Value, out int numeroLote))
                        Query.Where(x => x.NumeroLote == numeroLote);
                    break;
                case "codigoorganismo":
                    if (int.TryParse(filter.Value, out int codigoOrganismo))
                        Query.Where(x => x.CodigoOrganismo == codigoOrganismo);
                    break;
                case "tiposervico":
                    if (int.TryParse(filter.Value, out int tipoServico))
                        Query.Where(x => x.TipoServico == tipoServico);
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(dynamicOrder))
            Query.OrderByDescending(x => x.DataLote).ThenByDescending(x => x.Indice);
        else
            Query.OrderBy(dynamicOrder);
    }
}
