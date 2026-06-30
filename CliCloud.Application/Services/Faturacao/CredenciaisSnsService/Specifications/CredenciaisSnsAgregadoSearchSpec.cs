using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Credenciais;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Specifications;

public class CredenciaisSnsAgregadoSearchSpec : Specification<LoteDirectAgregado>
{
    public CredenciaisSnsAgregadoSearchSpec(string modulo, List<TableFilter> filters, string? dynamicOrder = "")
    {
        // Ponto 2: especialidades = agregados de consultas (único pipeline activo).
        // Ponto 7: filtrar por TipoServico / regra fisioterapia.
        _ = modulo;

        foreach (TableFilter filter in filters ?? [])
        {
            switch ((filter.Id ?? "").ToLowerInvariant())
            {
                case "filtrobox":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        string termo = filter.Value.Trim();
                        if (int.TryParse(termo, out int indiceOuLote))
                            Query.Where(x => x.Indice == indiceOuLote || x.NumeroLote == indiceOuLote);
                        else
                            Query.Where(x => x.NumeroLote.ToString().Contains(termo));
                    }
                    break;
                case "datalotede":
                    if (DateTime.TryParse(filter.Value, out DateTime de))
                        Query.Where(x => x.DataLote >= de);
                    break;
                case "dataloteate":
                    if (DateTime.TryParse(filter.Value, out DateTime ate))
                        Query.Where(x => x.DataLote <= ate);
                    break;
                case "numerolotede":
                    if (int.TryParse(filter.Value, out int loteDe))
                        Query.Where(x => x.NumeroLote >= loteDe);
                    break;
                case "numeroloteate":
                    if (int.TryParse(filter.Value, out int loteAte))
                        Query.Where(x => x.NumeroLote <= loteAte);
                    break;
                case "codigoorganismode":
                    if (int.TryParse(filter.Value, out int orgDe))
                        Query.Where(x => x.CodigoOrganismo >= orgDe);
                    break;
                case "codigoorganismoate":
                    if (int.TryParse(filter.Value, out int orgAte))
                        Query.Where(x => x.CodigoOrganismo <= orgAte);
                    break;
                case "anode":
                    if (int.TryParse(filter.Value, out int anoDe))
                        Query.Where(x => x.Ano >= anoDe);
                    break;
                case "anoate":
                    if (int.TryParse(filter.Value, out int anoAte))
                        Query.Where(x => x.Ano <= anoAte);
                    break;
                case "mesde":
                    if (int.TryParse(filter.Value, out int mesDe))
                        Query.Where(x => x.Mes >= mesDe);
                    break;
                case "mesate":
                    if (int.TryParse(filter.Value, out int mesAte))
                        Query.Where(x => x.Mes <= mesAte);
                    break;
                case "ano":
                    if (int.TryParse(filter.Value, out int ano))
                        Query.Where(x => x.Ano == ano);
                    break;
                case "mes":
                    if (int.TryParse(filter.Value, out int mes))
                        Query.Where(x => x.Mes == mes);
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(dynamicOrder))
            Query.OrderBy(x => x.NumeroLote);
        else
            Query.OrderBy(dynamicOrder);
    }
}