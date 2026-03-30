using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.Filters
{
  public class QuestionarioUtenteTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

