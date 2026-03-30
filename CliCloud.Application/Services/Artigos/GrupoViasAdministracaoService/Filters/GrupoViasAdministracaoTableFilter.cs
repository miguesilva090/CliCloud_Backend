using System.Collections.Generic;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Filters
{
    public class GrupoViasAdministracaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
