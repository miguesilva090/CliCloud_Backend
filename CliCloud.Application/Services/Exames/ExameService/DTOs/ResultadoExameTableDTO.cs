using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  /// <summary>
  /// Linha de resultado de exame apresentada na ficha clínica (Resultados de Exames).
  /// Nesta primeira fase é derivada das linhas da prescrição (ExameLinha).
  /// Quando existir entidade própria de resultados poderá ser ajustado.
  /// </summary>
  public class ResultadoExameTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ExameId { get; set; }
    public Guid TipoExameId { get; set; }

    public string? NomeExame { get; set; }
    public int Quantidade { get; set; }

    // Campos de resultado efetivo
    public string? Valor { get; set; }
    public string? Referencia { get; set; }
    public string? Obs { get; set; }
  }
}

