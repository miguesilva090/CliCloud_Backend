using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

public class ExameSemPapelFiltroRequest : PaginationFilter
{
  public string? SearchUtente { get; set; }
  public DateTime? DataInicio { get; set; }
  public DateTime? DataFim { get; set; }
  public bool PorAssinar { get; set; }
  public bool ApenasEfetuadosNaoPrescritos { get; set; }
}
