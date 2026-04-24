#nullable enable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Domain.Entities.Servicos
{
  [Table("Servico", Schema = "Servicos")]
  public class Servico : AuditableEntityWithSoftDelete
  {
    [Required]
    [StringLength(250)]
    public string Designacao { get; set; } = string.Empty;
    
    public Guid TipoServicoId { get; set; }
    public TipoServico TipoServico { get; set; } = null!;
    
    public decimal? Preco { get; set; }
    public string? Duracao { get; set; }
  
    // Relação com Taxa de IVA
    public Guid? TaxaIvaId { get; set; }
    public TaxaIva? TaxaIva { get; set; }
    
    [StringLength(50)]
    public string? EAN { get; set; }
    
    public Guid? TipoAparelhoId { get; set; }
    public TipoAparelho? TipoAparelho { get; set; }
    
    // Flags
    public bool TratDentario { get; set; }
    
    // Motivo isenção (código legado, futura FK)
    public int? CodigoMotivoIsencao { get; set; }
 
    // Estado
    public bool Inativo { get; set; }

    // Relação 1-N com subsistemas/organismos (equivalente a ACOR_INS no legado)
    public ICollection<SubsistemaServico> Subsistemas { get; set; } = new List<SubsistemaServico>();
  }
}
