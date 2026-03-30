using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoSearchTable : Specification<Documento>
    {
        public DocumentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
          _ = Query.Include(x => x.TipoDocumento)
            .Include(x => x.Utente)
            .Include(x => x.Organismo)
            .Include(x => x.Funcionario);

            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "numerodocumento":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int numeroDocumento))
                    {
                      _ = Query.Where(x => x.NumeroDocumento == numeroDocumento);
                    }
                    break;
                  case "data":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime data))
                    {
                      _ = Query.Where(x => x.Data.HasValue && x.Data.Value.Date == data.Date);
                    }
                    break;
                  case "nomecliente":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NomeCliente != null && x.NomeCliente.Contains(filter.Value));
                    }
                    break;
                  case "numeroContribuinteCliente":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.NumeroContribuinteCliente != null && x.NumeroContribuinteCliente.Contains(filter.Value));
                    }
                    break;
                  case "tipodocumentoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid tipoDocumentoId))
                    {
                      _ = Query.Where(x => x.TipoDocumentoId == tipoDocumentoId);
                    }
                    break;
                  case "utenteid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid utenteId))
                    {
                      _ = Query.Where(x => x.UtenteId == utenteId);
                    }
                    break;
                  case "organismoid":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid organismoId))
                    {
                      _ = Query.Where(x => x.OrganismoId == organismoId);
                    }
                    break;
                  case "estado":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && int.TryParse(filter.Value, out int estado))
                    {
                      _ = Query.Where(x => x.Estado == estado);
                    }
                    break;
                  case "liquidado":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool liquidado))
                    {
                      _ = Query.Where(x => x.Liquidado == liquidado);
                    }
                    break;
                  case "condicaopagamento":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && Enum.TryParse<CondicaoPagamento>(filter.Value, true, out CondicaoPagamento condicaoPagamento))
                    {
                      _ = Query.Where(x => x.CondicaoPagamento == condicaoPagamento);
                    }
                    break;
                  default:
                    break;
                }
              }
            }

            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
