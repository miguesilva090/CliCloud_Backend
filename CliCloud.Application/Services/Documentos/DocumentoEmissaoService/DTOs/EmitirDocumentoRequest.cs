#nullable enable

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class EmitirDocumentoRequest : IDto 
{
    [Required]
    public required Guid TipoDocumentoId { get; set; }

    [Required]
    public required int AnoFiscal { get; set; }

    public DateTime? DataDocumento { get; set; }

    public Guid? UtenteId { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? FuncionarioId { get; set; }

    [Required]
    public required string NomeCliente { get; set; }
    
    [Required] 
    public required string MoradaCliente { get; set; }
    
    public string? LocalidadeCliente { get; set; }
    public string? NumeroContribuinteCliente { get; set; }
    public Guid? CodigoPostalId { get; set; }

    public CondicaoPagamento? CondicaoPagamento { get; set; }
    public TipoModoPagamento? TipoModoPagamento { get; set; }
    public Guid? MoedaId { get; set; }
    public Guid? BancoId { get; set; }
    public decimal? TaxaCambio { get; set; }
    public int? TipoCambio { get; set; }
    public DateTime? DataVencimentoPagamento { get; set; }

    public decimal? DescontoCliente { get; set; }
    public decimal? DescontoPagamento { get; set; }
    public decimal? Outros { get; set; }
    
    public bool IsentoIva { get; set; }
    public bool IvaCaixa { get; set; }

    public bool Rectificado { get; set; }
    public bool Liquidado { get; set; }
    public bool Anulado { get; set; }

    public int? CaixaId { get; set; }
    public string? Observacoes { get; set; }

    public string? CodigoValidacaoTransporte { get; set; }
    public DateTime? DataTransporte { get; set; }
    public string? HoraTransporte { get; set; }

    public ModuloOrigemDocumento? ModuloOrigem { get; set; }

    public int? CodigoTipoDocSaft { get; set; }

    public List<EmitirDocumentoLinhaRequest> Linhas { get; set; } = [];

    


}