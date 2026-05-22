namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

/// <summary>Evento vista mês — legado ObterDisponibilidadeMedicosMesAtualPorEspecialidade.</summary>
public class DisponibilidadeMedicoDiaEventoDTO
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public Guid MedicoId { get; set; }
  public string Start { get; set; } = string.Empty;
  public string End { get; set; } = string.Empty;
}
