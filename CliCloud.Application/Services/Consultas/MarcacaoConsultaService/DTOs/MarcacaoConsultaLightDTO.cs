using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs
{
  public class MarcacaoConsultaLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public DateTime? Data { get; set; }
    public TimeSpan? HoraMarcacao { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
  }
}

