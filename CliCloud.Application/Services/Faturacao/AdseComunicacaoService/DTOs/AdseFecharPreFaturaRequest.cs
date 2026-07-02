namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdseFecharPreFaturaRequest
{
    public string ReferenciaSerie { get; set; } = string.Empty;
    public int ReferenciaNumeroDocumento { get; set; }
    public DateTime ReferenciaData { get; set; }
    public decimal? ReferenciaValor { get; set; }
    public string? PdfFicheiro { get; set; }
}
