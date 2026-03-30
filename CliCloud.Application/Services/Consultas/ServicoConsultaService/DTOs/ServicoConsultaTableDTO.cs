using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs
{
  public class ServicoConsultaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ConsultaId { get; set; }
    public Guid? ServicoId { get; set; }
    public Guid? ExameId { get; set; }
    public int Linha { get; set; }
    public string? CodigoArtigo { get; set; }
    public string? NomeArtigo { get; set; }
    public decimal? Quantidade { get; set; }
    public decimal? ValorArtigo { get; set; }
    public decimal? ValorServico { get; set; }
    public decimal? ValorUt { get; set; }
    public int? Ordem { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}

