using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
  /// <summary>
  /// Projeção mínima de Id + NumeroUtente (mesma origem que a listagem paginada de utentes).
  /// </summary>
  public class UtenteNumeroLookupDTO : IDto
  {
    public Guid Id { get; set; }
    public string? NumeroUtente { get; set; }
  }
}
