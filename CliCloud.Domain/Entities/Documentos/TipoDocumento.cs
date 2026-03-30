#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Documentos
{
  [Table("TipoDocumento", Schema = "Documentos")]
  public class TipoDocumento : AuditableEntity
  {
    [Required]
    [StringLength(50)]
    public string Descricao { get; set; } = string.Empty;
    
    [Required]
    [StringLength(5)]
    public string Abreviatura { get; set; } = string.Empty;
    
    // Natureza e tipo
    [StringLength(1)]
    public string? Natureza { get; set; } 
    
    public int? TipoMovimento { get; set; } 
    
    // Série e numeração
    [StringLength(14)]
    public string? NumeroSerie { get; set; }
    
    [StringLength(20)]
    public string? TipoSerie { get; set; }
    
    public int? NumeroDocumento { get; set; }
    
    // Configuração do documento
    public int? NumVias { get; set; }
    public int? TemCabecalho { get; set; } 
    public int? ImprimirEmtodasAsVias { get; set; } 
    public int? PermiteMovimento { get; set; } 
    public int? Config { get; set; }
    
    // Stock
    public int? AtualizaStock { get; set; } 
    
    // Estado e visibilidade
    public bool Inactivo { get; set; }
    public bool MostraFaturacao { get; set; }
    public bool DescarregarTesouraria { get; set; }
    public bool Habilitado { get; set; }
    
    // CAE (Classificação de Atividades Económicas)
    [StringLength(10)]
    public string? Cae { get; set; }
    
    // ATCUD (Autorização de Transmissão de Comunicados de Documentos)
    [StringLength(50)]
    public string? CodigoATCUD { get; set; }
    
    [StringLength(1)]
    public string? ATCUDEstado { get; set; } 
    
    public DateTime? ATCUDData { get; set; }
    
    // Integração Contabilidade
    [StringLength(10)]
    public string? ContabContaConsulta { get; set; }
    
    [StringLength(10)]
    public string? ContabContaTratamento { get; set; }
    
    [StringLength(10)]
    public string? ContabContaOutros { get; set; }
    
    public int? ContabTipoServicoConsulta { get; set; }
    public int? ContabTipoServicoTratamento { get; set; }
    
    [StringLength(20)]
    public string? ContabTipoConta { get; set; } 
    
    [StringLength(10)]
    public string? ContabDiario { get; set; }
    
    [StringLength(10)]
    public string? ContabSeccao { get; set; }
    
    [StringLength(20)]
    public string? ContabSerieSeccao { get; set; }
    
    [StringLength(20)]
    public string? ContabDimensaoCCDebito { get; set; }
    
    [StringLength(20)]
    public string? ContabDimensaoCCCredito { get; set; }
    
    [StringLength(50)]
    public string? ContabValorCCDebito { get; set; }
    
    [StringLength(50)]
    public string? ContabValorCCCredito { get; set; }
    
    // Integração POCAL (sistema externo)
    public int? PocalTipoDocumento { get; set; }
    public int? PocalGuiaFatAutartica { get; set; }
    public int? PocalCodigoServico { get; set; }
    public int? PocalTipoDocPocalEmitido { get; set; }
    public int? PocalTipoDocPocalCobrado { get; set; }
    
    // Relatório personalizado
    [StringLength(100)]
    public string? ReportPersonalizado { get; set; }
  }
}
