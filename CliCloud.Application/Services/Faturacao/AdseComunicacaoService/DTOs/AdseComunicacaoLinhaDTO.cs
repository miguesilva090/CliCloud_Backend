using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdseComunicacaoLinhaDTO : IDto
{
    public string Id { get; set; } = string.Empty;
    public Guid OrigemClinicaId { get; set; }
    public Guid DocumentoId { get; set; }
    public Guid? CoPagamentoId { get; set; }
    /// <summary>Legado: NUM_TR (identificador da origem clínica).</summary>
    public string NumeroOrigem { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int NumeroSessoes { get; set; }
    public Guid UtenteId { get; set; }
    public string UtenteNome { get; set; } = string.Empty;
    public string NumeroFatura { get; set; } = string.Empty;
    public DateTime? DataFatura { get; set; }
    public decimal ValorFatura { get; set; }
    public decimal ValorAdse { get; set; }
    public string? PreFatura { get; set; }
    public string? FaturaAdse { get; set; }
    public int Estado { get; set; }
    public string EstadoDescricao { get; set; } = string.Empty;
    public DateTime? DataComunicacao { get; set; }
    public string? PdfFicheiro { get; set; }
    public string? PdfRelatorioFicheiro { get; set; }
    public string? Erros { get; set; }
    public string? NumeroDevolucao { get; set; }
}
