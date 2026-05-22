using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AdmissaoObservacoesDTO : IDto
{
  public string Observacoes { get; set; } = string.Empty;
}
