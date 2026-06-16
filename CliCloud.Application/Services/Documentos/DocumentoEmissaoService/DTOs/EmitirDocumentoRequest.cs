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

    /// <summary>Legado TFatura.Beneficiario.</summary>
    [StringLength(100)]
    public string? Beneficiario { get; set; }

    /// <summary>Legado TFatura.GlobalDataDe / GlobalDataAte (preenchido por fatura global).</summary>
    public DateTime? FaturaGlobalDataInicio { get; set; }

    public DateTime? FaturaGlobalDataFim { get; set; }

    public Guid? CondicaoPagamentoId { get; set; }
    public Guid? ModoPagamentoId { get; set; }
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
    public Guid? MotivoIsencaoId { get; set; }

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

    /// <summary>N/D/M na emissão; por defeito usa o do tipo de documento.</summary>
    [StringLength(1)]
    public string? TipoSerie { get; set; }

    public List<EmitirDocumentoLinhaRequest> Linhas { get; set; } = [];

    public bool RetencaoAtiva { get; set; }
    public string? RetencaoImposto { get; set; }
    public decimal? RetencaoTaxa { get; set; }
    public decimal? RetencaoValor { get; set; }

    public int? RetencaoCodigoMotivo { get; set; }

    public string? RetencaoMotivo { get; set; }

    public decimal? PercentagemDescontoGlobal { get; set; }
    
    public Guid? DocumentoOrigemId { get; set; }
    public string? IdentificadorUnicoDocumentoOrigem { get; set; }
    public DateTime? DataDocumentoOrigem { get; set; }
    public int? GerarReferenciaMb { get; set; }

    /// <summary>Sinistro associado (legado modFldCodigoSinistrado).</summary>
    public Guid? SinistradoId { get; set; }

    /// <summary>Override sync clínica ao emitir desde admissão/consulta (ex.: FR → pago=true).</summary>
    public bool? AdmissaoSyncPago { get; set; }

    public bool? AdmissaoSyncFaturado { get; set; }

}