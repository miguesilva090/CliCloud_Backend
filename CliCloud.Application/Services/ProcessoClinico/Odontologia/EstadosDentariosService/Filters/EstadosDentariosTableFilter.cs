using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Filters
{
    public class EstadosDentariosTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
