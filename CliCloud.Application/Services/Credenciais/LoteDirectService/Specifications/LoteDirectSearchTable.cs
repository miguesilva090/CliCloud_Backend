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
                        if (int.TryParse(filter.Value, out var indiceLote))
                        {
                            // Paridade legado: ?indice= do SNS filtra pelo índice do agregado (LOTESP),
                            // não só pelo IndiceLote do cabeçalho (pode estar null antes de corrigir lotes).
                            Query.Where(x =>
                                x.IndiceLote == indiceLote
                                || x.Detalhes.Any(d => d.Indice == indiceLote));
                        }
                        break;
                    case "utentenumero_de":
                    case "c_utente_de":
                        if (int.TryParse(filter.Value, out var utenteNumeroDe))
                            Query.Where(x =>
                                x.Utente != null
                                && x.Utente.NumeroUtente != null
                                && Convert.ToInt32(x.Utente.NumeroUtente) >= utenteNumeroDe);
                        break;
                    case "utentenumero_ate":
                    case "c_utente_ate":
                        if (int.TryParse(filter.Value, out var utenteNumeroAte))
                            Query.Where(x =>
                                x.Utente != null
                                && x.Utente.NumeroUtente != null
                                && Convert.ToInt32(x.Utente.NumeroUtente) <= utenteNumeroAte);
                        break;
                    case "utentenome":
                    case "nome":
                        if (!string.IsNullOrWhiteSpace(filter.Value))
                            Query.Where(x => x.Utente != null && x.Utente.Nome.Contains(filter.Value));
                        break;
                    case "datafim_de":
                        if (DateTime.TryParse(filter.Value, out var dataFimDe))
                            Query.Where(x => x.DataFim.HasValue && x.DataFim.Value.Date >= dataFimDe.Date);
                        break;
                    case "datafim_ate":
                        if (DateTime.TryParse(filter.Value, out var dataFimAte))
                            Query.Where(x => x.DataFim.HasValue && x.DataFim.Value.Date <= dataFimAte.Date);
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