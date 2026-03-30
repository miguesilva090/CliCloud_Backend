using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Atestados.AtestadoService.DTOs
{
  public class AtestadoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public string? NomeUtente { get; set; }
    public Guid MedicoId { get; set; }
    public string? NomeMedico { get; set; }
    public Guid ClinicaId { get; set; }
    public DateTime DataAtestado { get; set; }
    public string? NumeroSPMS { get; set; }
    public int EstadoEnvio { get; set; }
    public DateTime? DataEnvio { get; set; }
    public string? Observacoes { get; set; }
    public string? NumeroSNS { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}
