using System.Text.Json.Serialization;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Filters
{
    public class EvolucaoTratamentoTableFilter : PaginationFilter
    {
        [JsonPropertyName("filters")]
        public List<TableFilter> Filters { get; set; }

        public EvolucaoTratamentoTableFilter()
        {
            Filters = [];
        }
    }
}
