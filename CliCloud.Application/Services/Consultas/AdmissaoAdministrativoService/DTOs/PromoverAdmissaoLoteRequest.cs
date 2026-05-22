using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class PromoverAdmissaoLoteRequest : IDto
{
  public List<Guid> Ids { get; set; } = [];
}
