using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Filters
{
    public class AvaliacaoAntropometricaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
