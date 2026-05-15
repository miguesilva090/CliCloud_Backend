using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AdmissaoServicoDTO : IDto
{
  public Guid? Id { get; set; }
  public Guid? ServicoId { get; set; }
  public decimal? ValorServico { get; set; }
  public string? CodigoArtigo { get; set; }
  public string? NomeArtigo { get; set; }
  public decimal? ValorArtigo { get; set; }
  public decimal? Quantidade { get; set; }
  public decimal? MargemMed { get; set; }
  public decimal? MargemIns { get; set; }
  public decimal? RecMed { get; set; }
  public decimal? RecInst { get; set; }
  public decimal? DescInst { get; set; }
  public decimal? DescCli { get; set; }
  public decimal? ValorDesc { get; set; }
  public int? Ordem { get; set; }
  public string? Dente { get; set; }
  public Guid? ExameId { get; set; }
  public int Linha { get; set; }
  public string? NCheque { get; set; }
  public int? Electrocardiograma { get; set; }
  public decimal? ValorUt { get; set; }
}
