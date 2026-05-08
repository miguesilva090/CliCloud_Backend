namespace CliCloud.Application.Services.Consultas.SalaService.DTOs
{
  public class CreateSalaRequest
  {
    public string Nome { get; set; } = string.Empty;
    public int NumeroSala { get; set; }
    public Guid ClinicaId { get; set; }
    public bool Ativa { get; set; } = true;
  }
}
