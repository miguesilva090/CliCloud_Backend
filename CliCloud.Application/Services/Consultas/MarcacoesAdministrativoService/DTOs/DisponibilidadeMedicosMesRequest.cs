namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class DisponibilidadeMedicosMesRequest
{
  public Guid EspecialidadeId { get; set; }
  public int Mes { get; set; }
  public int Ano { get; set; }
}
