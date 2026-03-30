using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Filters
{
    public class DocumentoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public DocumentoTableFilter()
        {
            Filters = [];
        }
    }
}
