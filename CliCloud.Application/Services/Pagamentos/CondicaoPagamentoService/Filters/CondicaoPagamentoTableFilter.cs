using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Filters;

public class CondicaoPagamentoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
    public string? FiltroBox { get; set; }
    public int? CodigoDe { get; set; }
    public int? CodigoAte { get; set; }
    public string? DescricaoDe { get; set; }
    public string? DescricaoAte { get; set; }
}
