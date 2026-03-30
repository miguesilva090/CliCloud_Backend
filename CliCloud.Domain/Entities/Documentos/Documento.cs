#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Documentos
{
  [Table("Documento", Schema = "Documentos")]
  public class Documento : AuditableEntity
  {
    // Identificação do documento
    public Guid TipoDocumentoId { get; set; }
    public TipoDocumento? TipoDocumento { get; set; }
    public int NumeroDocumento { get; set; }
    
    // Data do documento
    public DateTime? Data { get; set; }
    
    // Relacionamentos principais (cliente pode ser Utente ou Organismo)
    public Guid? UtenteId { get; set; }
    public Utente? Utente { get; set; }
    
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    
    public Guid? FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
    
    // Valores do documento
    public decimal? DescontoCliente { get; set; }
    public decimal? TotalDocumento { get; set; }
    public decimal? TotalIva { get; set; }
    public decimal? TotalDesconto { get; set; }
    public decimal? TotalLiquido { get; set; }
    public decimal? Outros { get; set; }
    
    // Condições de pagamento
    public CondicaoPagamento? CondicaoPagamento { get; set; }
    public TipoModoPagamento? TipoModoPagamento { get; set; }
    
    // Observações
    [StringLength(250)]
    public string? Observacoes { get; set; }
    
    // Estado e controlo
    public int? Estado { get; set; }
    public bool Liquidado { get; set; }
    public bool Rectificado { get; set; }
    public bool Exportado { get; set; }
    public bool IsentoIva { get; set; }
    
    
    [StringLength(50)]
    public string? NomeCliente { get; set; }
    [StringLength(50)]
    public string? MoradaCliente { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public CodigoPostal? CodigoPostal { get; set; }
    [StringLength(40)]
    public string? LocalidadeCliente { get; set; }
    [StringLength(20)]
    public string? NumeroContribuinteCliente { get; set; }
    
    // Outros campos comuns
    public int? NumVias { get; set; }
    public int? Emitido { get; set; }
    public int? Origem { get; set; }
    
    // ATCUD (Autorização de Transmissão de Comunicados de Documentos)
    [StringLength(250)]
    public string? GlobalHash { get; set; }
    public int? VersaoChave { get; set; }
    
    // Data de registo no sistema
    public DateTime? DataSistemaRegisto { get; set; }
  }
}
