namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MarcacaoCalendarioRequest
{
  public Guid MedicoId { get; set; }
  public Guid? SalaId { get; set; }
  public Guid? EspecialidadeId { get; set; }
  public DateTime DataDe { get; set; }
  public DateTime DataAte { get; set; }
}
