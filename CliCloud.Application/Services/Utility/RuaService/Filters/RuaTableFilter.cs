using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.RuaService.Filters
{
    public class RuaTableFilter : PaginationFilter
    {
       public List<TableFilter> Filters { get; set; }

       public RuaTableFilter()
       {
        Filters = [];
       }
    }
}
