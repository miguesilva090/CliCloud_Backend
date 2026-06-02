#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Moedas;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Documentos;

/// <summary>
/// Cabeçalho de documento fiscal (fatura, recibo, nota de crédito, etc.).
/// Equivalente legado: <c>Faturacao.TFatura</c>.
/// </summary>
[Table("Documento", Schema = "Documentos")]
public class Documento : AuditableEntityWithSoftDelete
{
  // --- Contexto de emissão ---
  public Guid ClinicaId { get; set; }
  public Clinica? Clinica { get; set; }

  /// <summary>Ano fiscal / época (legado CodigoEpoca).</summary>
  public int AnoFiscal { get; set; }

  public Guid TipoDocumentoId { get; set; }
  public TipoDocumento? TipoDocumento { get; set; }

  /// <summary>Número sequencial na série (legado NumeroDocumento).</summary>
  public int NumeroDocumento { get; set; }

  /// <summary>Número apresentado ao utilizador (legado NumeroTFatura), ex. ATCUD-123.</summary>
  [StringLength(50)]
  public string? NumeroExibicao { get; set; }

  public DateTime? Data { get; set; }

  public DateTime? DataSistemaRegisto { get; set; }

  // --- Destinatário (snapshot fiscal) ---
  public Guid? UtenteId { get; set; }
  public Utente? Utente { get; set; }

  public Guid? OrganismoId { get; set; }
  public Organismo? Organismo { get; set; }

  public Guid? FuncionarioId { get; set; }
  public Funcionario? Funcionario { get; set; }

  [StringLength(100)]
  public string? NomeCliente { get; set; }

  [StringLength(100)]
  public string? MoradaCliente { get; set; }

  public Guid? CodigoPostalId { get; set; }
  public CodigoPostal? CodigoPostal { get; set; }

  [StringLength(40)]
  public string? LocalidadeCliente { get; set; }

  [StringLength(20)]
  public string? NumeroContribuinteCliente { get; set; }

  // --- Totais ---
  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalBruto { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalDocumento { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalIva { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalDesconto { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalLiquido { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? DescontoCliente { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? DescontoPagamento { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? Outros { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? PrecoUnitarioMercadorias { get; set; }

  // --- Pagamento ---
  public CondicaoPagamento? CondicaoPagamento { get; set; }
  public TipoModoPagamento? TipoModoPagamento { get; set; }

  public Guid? MoedaId { get; set; }
  public Entities.Moedas.Moeda? Moeda { get; set; }

  [Column(TypeName = "decimal(18,6)")]
  public decimal? TaxaCambio { get; set; }

  public int? TipoCambio { get; set; }

  public DateTime? DataVencimentoPagamento { get; set; }

  public Guid? BancoId { get; set; }
  public Banco? Banco { get; set; }

  // --- Estado ---
  /// <summary>Legado: coluna Estado (int). Preferir <see cref="EstadoDocumento"/> em código novo.</summary>
  public int? Estado { get; set; }

  public EstadoDocumento? EstadoDocumento { get; set; }

  public bool Liquidado { get; set; }
  public bool Rectificado { get; set; }
  public bool Exportado { get; set; }
  public bool IsentoIva { get; set; }
  public bool Anulado { get; set; }
  public bool IvaCaixa { get; set; }
  public Guid? MotivoIsencaoId { get; set; }
  public MotivoIsencao? MotivoIsencao { get; set; }

  /// <summary>Legado: Emitido como int (0/1). Preferir <see cref="EstaEmitido"/>.</summary>
  public int? Emitido { get; set; }

  public bool EstaEmitido { get; set; }

  /// <summary>Legado: coluna Origem. Preferir <see cref="ModuloOrigem"/>.</summary>
  public int? Origem { get; set; }

  public ModuloOrigemDocumento? ModuloOrigem { get; set; }

  public int? NumVias { get; set; }

  /// <summary>Modo de emissão na criação: N=normal, D=duplicado, M=manual (legado TipoSerie).</summary>
  [StringLength(1)]
  public string? TipoSerie { get; set; }

  [StringLength(250)]
  public string? Observacoes { get; set; }

  // --- Comunicação fiscal (PT) ---
  [StringLength(50)]
  public string? CodigoAtcud { get; set; }

  [StringLength(250)]
  public string? GlobalHash { get; set; }

  public int? VersaoChave { get; set; }

  public Guid? DocumentoOrigemId { get; set; }
  public Documento? DocumentoOrigem { get; set; }

  [StringLength(100)]
  public string? IdentificadorUnicoDocumentoOrigem { get; set; }

  public DateTime? DataDocumentoOrigem { get; set; }

  [StringLength(250)]
  public string? HashDocumentoOrigem { get; set; }

  // --- Anulação ---
  [StringLength(250)]
  public string? MotivoAnulacao { get; set; }

  public DateTime? DataAnulacao { get; set; }

  // --- Retenção na fonte ---
  [StringLength(20)]
  public string? RetencaoImposto { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? RetencaoTaxa { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? RetencaoValor { get; set; }

  public int? RetencaoCodigoMotivo { get; set; }

  [StringLength(250)]
  public string? RetencaoMotivo { get; set; }

  // --- Transporte ---
  [StringLength(50)]
  public string? CodigoValidacaoTransporte { get; set; }

  public DateTime? DataTransporte { get; set; }

  [StringLength(10)]
  public string? HoraTransporte { get; set; }

  // --- Fatura global ---
  public DateTime? FaturaGlobalDataInicio { get; set; }

  public DateTime? FaturaGlobalDataFim { get; set; }

  [StringLength(100)]
  public string? BeneficiarioFaturaGlobal { get; set; }

  // --- Operacional ---
  public int? CaixaId { get; set; }

  public string? QrCodeRecibo { get; set; }

  // --- Navegação ---
  public ICollection<DocumentoLinha> Linhas { get; set; } = [];

  public DocumentoOrigemClinica? OrigemClinica { get; set; }

  public ICollection<Documento> DocumentosDerivados { get; set; } = [];
}
