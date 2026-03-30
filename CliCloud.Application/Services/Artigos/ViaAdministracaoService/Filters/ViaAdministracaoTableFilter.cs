using System.Collections.Generic;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.Filters
{
    public class ViaAdministracaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
