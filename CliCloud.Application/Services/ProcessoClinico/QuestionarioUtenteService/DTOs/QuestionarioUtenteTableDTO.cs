using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs
{
  public class QuestionarioUtenteTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public DateTime DataCriacao { get; set; }
  }
}

