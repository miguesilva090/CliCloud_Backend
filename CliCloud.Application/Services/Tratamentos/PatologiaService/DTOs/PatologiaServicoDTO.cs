using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class PatologiaServicoDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid PatologiaId { get; set; }
    public Guid SubsistemaServicoId { get; set; }
    public string? CodigoServico { get; set; }
    public string? DescricaoServico { get; set; }
    public string? SubSistema { get; set; }
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
