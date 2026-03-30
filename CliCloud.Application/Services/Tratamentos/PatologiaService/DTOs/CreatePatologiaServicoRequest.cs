using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class CreatePatologiaServicoRequest : IDto
  {
    public Guid SubsistemaServicoId { get; set; }
    public string? Duracao { get; set; }
    public int Ordem { get; set; }
    public bool Fisioterapia { get; set; }
    public bool Auxiliar { get; set; }
    public decimal ValorUtente { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal? PercentagemInstituicao { get; set; }
    public decimal? PrecoEur { get; set; }
    public string? Observacoes { get; set; }
  }
}
