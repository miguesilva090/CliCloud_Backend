using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;

public class FechoDiarioResultDTO : IDto
{
  public int TotalProcessadas { get; set; }
  public int TotalConsultasCriadas { get; set; }
  public List<string> Erros { get; set; } = [];
}
