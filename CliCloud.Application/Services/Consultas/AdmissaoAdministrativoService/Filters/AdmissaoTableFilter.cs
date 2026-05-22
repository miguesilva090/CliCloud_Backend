using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;

public class AdmissaoTableFilter : PaginationFilter
{
  public ModoListagemAdmissao Modo { get; set; } = ModoListagemAdmissao.Dia;
  public DateTime? DataReferencia { get; set; }
  public List<TableFilter>? Filters { get; set; }
}
