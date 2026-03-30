using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Atestados.AtestadoService.DTOs
{
  public class AtestadoDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid ClinicaId { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public DateTime DataAtestado { get; set; }
    public string? NumeroSPMS { get; set; }
    public int EstadoEnvio { get; set; }
    public DateTime? DataEnvio { get; set; }
    public string? Observacoes { get; set; }
    public string? NumeroSNS { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
