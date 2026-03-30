using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  public class ExameLinhaDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TipoExameId { get; set; }
    public string? TipoExameDesignacao { get; set; }
    public int Quantidade { get; set; }
    public string? Recomendacoes { get; set; }
    public string? ResultadoValor { get; set; }
    public string? ResultadoReferencia { get; set; }
    public string? ResultadoObs { get; set; }
  }
}
