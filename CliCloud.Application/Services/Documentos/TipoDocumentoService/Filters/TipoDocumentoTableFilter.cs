using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Filters
{
    public class TipoDocumentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public TipoDocumentoTableFilter()
        {
            Filters = [];
        }
    }
}
