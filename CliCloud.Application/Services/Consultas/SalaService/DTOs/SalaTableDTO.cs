using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.SalaService.DTOs
{
  public class SalaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int NumeroSala { get; set; }
    public bool Ativa { get; set; }
    public Guid ClinicaId { get; set; }
    public string? ClinicaNome { get; set; }
  }
}
