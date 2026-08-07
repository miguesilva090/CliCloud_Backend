using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Filters;

public class AdmissaoTratamentoTableFilter : PaginationFilter
{
  public List<TableFilter> Filters { get; set; } = [];

  public ModoListagemAdmissaoTratamento Modo { get; set; } =
    ModoListagemAdmissaoTratamento.UtentesHora;

  /// <summary>Dia de trabalho (legado data_de / DataTrabalho).</summary>
  public DateTime? DataReferencia { get; set; }

  public Guid? LocalTratamentoId { get; set; }
  public Guid? FisioterapeutaId { get; set; }
  public Guid? UtenteId { get; set; }

  public bool IncluirDesmarcados { get; set; }
}
