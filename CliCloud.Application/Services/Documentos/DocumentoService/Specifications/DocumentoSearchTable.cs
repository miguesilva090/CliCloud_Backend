using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Enums;
namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoSearchTable : Specification<Documento>
    {
        public DocumentoSearchTable(List<TableFilter> filters, Guid? clinicaId = null, string? dynamicOrder = "")
        {
            _ = Query
                .Include(x => x.TipoDocumento)
                .Include(x => x.Utente)
                .Include(x => x.Organismo)
                .Include(x => x.Funcionario)
                .Include(x => x.DocumentoOrigem!)
                    .ThenInclude(d => d.TipoDocumento)
                .Include(x => x.OrigemClinica!)
                    .ThenInclude(o => o.Admissao)
                .Include(x => x.Linhas)
                    .ThenInclude(l => l.AdmissaoServico!)
                        .ThenInclude(a => a.Admissao)
                .Where(d =>
                    d.TipoDocumento == null
                    || (
                        (d.TipoDocumento.Abreviatura == null
                            || (
                                d.TipoDocumento.Abreviatura.ToUpper() != "RC"
                                && d.TipoDocumento.Abreviatura.ToUpper() != "FR"
                                && d.TipoDocumento.Abreviatura.ToUpper() != "REC"))
                        && (d.TipoDocumento.Descricao == null
                            || !d.TipoDocumento.Descricao.ToLower().Contains("recibo"))));

            if (clinicaId.HasValue)
            {
                _ = Query.Where(x => x.ClinicaId == clinicaId.Value);
            }

            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "numerodocumento":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int numeroDocumento))
                                _ = Query.Where(x => x.NumeroDocumento == numeroDocumento);
                            break;
                        case "numerodocumento_de":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int numeroDe))
                                _ = Query.Where(x => x.NumeroDocumento >= numeroDe);
                            break;
                        case "numerodocumento_ate":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int numeroAte))
                                _ = Query.Where(x => x.NumeroDocumento <= numeroAte);
                            break;
                        case "numeroexibicao":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NumeroExibicao != null && x.NumeroExibicao.Contains(filter.Value));
                            break;
                        case "numeroexibicao_de":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NumeroExibicao != null && string.Compare(x.NumeroExibicao, filter.Value) >= 0);
                            break;
                        case "numeroexibicao_ate":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NumeroExibicao != null && string.Compare(x.NumeroExibicao, filter.Value) <= 0);
                            break;
                        case "data":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime data))
                                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == data.Date);
                            break;
                        case "data_de":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime dataDe))
                                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date >= dataDe.Date);
                            break;
                        case "data_ate":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime dataAte))
                                _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date <= dataAte.Date);
                            break;
                        case "nomecliente":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NomeCliente != null && x.NomeCliente.Contains(filter.Value));
                            break;
                        case "nomecliente_de":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NomeCliente != null && string.Compare(x.NomeCliente, filter.Value) >= 0);
                            break;
                        case "nomecliente_ate":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NomeCliente != null && string.Compare(x.NomeCliente, filter.Value) <= 0);
                            break;
                        case "numerocontribuintecliente":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.NumeroContribuinteCliente != null && x.NumeroContribuinteCliente.Contains(filter.Value));
                            break;
                        case "tipodocumentoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid tipoDocumentoId))
                                _ = Query.Where(x => x.TipoDocumentoId == tipoDocumentoId);
                            break;
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid utenteId))
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            break;
                        case "organismoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid organismoId))
                                _ = Query.Where(x => x.OrganismoId == organismoId);
                            break;
                        case "estado":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int estado))
                                _ = Query.Where(x => x.Estado == estado);
                            break;
                        case "liquidado":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool liquidado))
                                _ = Query.Where(x => x.Liquidado == liquidado);
                            break;
                        case "anulado":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool anulado))
                                _ = Query.Where(x => x.Anulado == anulado);
                            break;
                        case "condicaopagamento":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<CondicaoPagamento>(filter.Value, true, out CondicaoPagamento condicaoPagamento))
                                _ = Query.Where(x => x.CondicaoPagamento == condicaoPagamento);
                            break;
                        case "siglaficheiro":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                string siglaFicheiro = filter.Value
                                    .Replace("/", "", StringComparison.Ordinal)
                                    .Replace("-", "", StringComparison.Ordinal)
                                    .ToUpperInvariant();

                                _ = Query.Where(x => x.UtenteId == null && x.OrganismoId != null);

                                if (siglaFicheiro is "SADGNR")
                                {
                                    _ = Query.Where(x => x.Organismo != null && x.Organismo.SADGNR);
                                }
                                else if (siglaFicheiro is "ADM")
                                {
                                    _ = Query.Where(x => x.Organismo != null && x.Organismo.ADM);
                                }
                                else if (siglaFicheiro is "SADPSP")
                                {
                                    _ = Query.Where(x => x.Organismo != null && x.Organismo.SADPSP);
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}