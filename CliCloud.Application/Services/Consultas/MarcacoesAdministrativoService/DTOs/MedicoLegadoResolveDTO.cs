using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MedicoLegadoResolveDTO : IDto
{
  public Guid MedicoId { get; set; }
  public string? MedicoNome { get; set; }
  public string? Letra { get; set; }
}
