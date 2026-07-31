using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Filters;

public class TratamentoMarcadosTableFilter : PaginationFilter
{
  public List<TableFilter> Filters { get; set; } = [];

  public ModoListagemTratamentoMarcados Modo { get; set; } =
    ModoListagemTratamentoMarcados.Marcados;

  public Guid? LocalTratamentoId { get; set; }
  public Guid? UtenteId { get; set; }
}
