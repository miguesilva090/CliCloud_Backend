using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  public class ExameLightDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime DataPrescricao { get; set; }
    public string? NumeroPrescricao { get; set; }
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
  }
}
