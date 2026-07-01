using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Filters
{
  public class ConfiguracaoADSETableFilter : PaginationFilter
  {
    public string? Keyword { get; set; }
    public Guid? EmpresaId { get; set; }
  }
}
