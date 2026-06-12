using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Filters
{
    public class NaturezaDocumentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
