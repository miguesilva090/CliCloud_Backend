using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdsePreFaturaDTO : IDto
{
    public Guid Id { get; set; }
    public string TipoPreFatura { get; set; } = string.Empty;
    public int NumOrdem { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int Estado { get; set; }
    public string EstadoDescricao { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFecho { get; set; }
    public decimal ValorTotal { get; set; }
    public int NumDocumentos { get; set; }
    public string? ReferenciaSerie { get; set; }
    public int? ReferenciaNumeroDocumento { get; set; }
    public DateTime? ReferenciaData { get; set; }
    public string? PdfFicheiro { get; set; }
    public string NumeroFaturaReferencia { get; set; } = string.Empty;
}
