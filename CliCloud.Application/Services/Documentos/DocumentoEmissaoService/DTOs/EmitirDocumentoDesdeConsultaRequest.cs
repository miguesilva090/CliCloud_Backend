#nullable enable

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class EmitirDocumentoDesdeConsultaRequest : IDto
{
    [Required]
    public required Guid TipoDocumentoId { get; set; }
    [Required]
    public required int AnoFiscal { get; set; }
    public DateTime? DataDocumento { get; set; }
    public DateTime? DataVencimentoPagamento { get; set; }
    public Guid? FuncionarioId { get; set; }
    public Guid? CondicaoPagamentoId { get; set; }
    public Guid? ModoPagamentoId { get; set; } 
    public Guid? MoedaId { get; set; }
    public Guid? BancoId { get; set; }
    public decimal? DescontoCliente { get; set; }
    public decimal? DescontoPagamento { get; set; }
    public decimal? Outros { get; set; }
    public bool IsentoIva { get; set; }
    public bool IvaCaixa { get; set; }
    public bool? Pago { get; set; }
    public bool? Faturado { get; set; }

    public int? CodigoTipoDocSaft { get; set; }

    public string? NomeCliente { get; set; }
    public string? MoradaCliente { get; set; }
    public string? LocalidadeCliente { get; set; }
    public string? NumeroContribuinteCliente { get; set; }

}