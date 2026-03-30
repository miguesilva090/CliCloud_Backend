using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoSearchTable : Specification<TipoDocumento>
    {
        public TipoDocumentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if(filters != null && filters.Count != 0)
            {
              foreach(TableFilter filter in filters)
              {
                switch((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                {
                  case "descricao":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Descricao.Contains(filter.Value));
                    }
                    break;
                  case "abreviatura":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Abreviatura.Contains(filter.Value));
                    }
                    break;
                  case "natureza":
                    if(!string.IsNullOrWhiteSpace(filter.Value))
                    {
                      _ = Query.Where(x => x.Natureza != null && x.Natureza.Contains(filter.Value));
                    }
                    break;
                  case "inactivo":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool inactivo))
                    {
                      _ = Query.Where(x => x.Inactivo == inactivo);
                    }
                    break;
                  case "mostrafaturacao":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool mostraFaturacao))
                    {
                      _ = Query.Where(x => x.MostraFaturacao == mostraFaturacao);
                    }
                    break;
                  case "habilitado":
                    if(!string.IsNullOrWhiteSpace(filter.Value) && bool.TryParse(filter.Value, out bool habilitado))
                    {
                      _ = Query.Where(x => x.Habilitado == habilitado);
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
                _ = Query.OrderBy(x => x.Descricao); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }
        }
    }
}
