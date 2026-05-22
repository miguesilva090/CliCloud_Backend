using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;

public class FechoDiarioResultDTO : IDto
{
  public int TotalElegiveis { get; set; }
  public int TotalProcessadas { get; set; }
  public int TotalConsultasCriadas { get; set; }
  public int TotalIgnoradas { get; set; }
  public List<string> Avisos { get; set; } = [];
  public List<string> Erros { get; set; } = [];
}
