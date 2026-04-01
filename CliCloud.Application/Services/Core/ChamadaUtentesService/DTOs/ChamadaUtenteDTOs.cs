using System;

namespace CliCloud.Application.Services.Core.ChamadaUtentesService.DTOs
{
  public class ChamadaUtenteDadosDTO
  {
    public string Tipo { get; set; } = string.Empty;
    public Guid ReferenciaId { get; set; }
    public string NomeUtente { get; set; } = string.Empty;
    public string? NomeProfissional { get; set; }
    public string? Sala { get; set; }
    public string? Senha { get; set; }
    public bool ExisteChamadaAtiva { get; set; }
    public bool ExisteChamadaFeita { get; set; }
  }

  public class ChamarConsultaRequest
  {
    public string? Sala { get; set; }
  }

  public class ChamarSessaoTratamentoRequest
  {
    public string? Sala { get; set; }
    public string? NomeTecnico { get; set; }
  }

  public class AtualizarEstadoChamadaRequest
  {
    public int Estado { get; set; } = 1;
  }

  public class ChamadaUtenteFilaItemDTO
  {
    public Guid Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public Guid ReferenciaId { get; set; }
    public string NomeUtente { get; set; } = string.Empty;
    public string? NomeProfissional { get; set; }
    public string? Sala { get; set; }
    public string? Senha { get; set; }
    public DateTime DataHoraChamada { get; set; }
    public int Estado { get; set; }
  }
}
