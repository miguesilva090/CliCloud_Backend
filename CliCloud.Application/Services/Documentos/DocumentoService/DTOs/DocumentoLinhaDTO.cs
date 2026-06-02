using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public class DocumentoLinhaDTO : IDto
{
    public Guid Id { get; set; }
    public int NumeroLinha { get; set; }
    public string? CodigoArtigo { get; set; }
    public Guid? ServicoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal? PercentagemDesconto { get; set; }
    public decimal? ValorDesconto { get; set; }
    public decimal? TotalLinha { get; set; }
    public decimal TaxaIvaPercentagem { get; set; }
    public decimal? ValorImposto { get; set; }
}