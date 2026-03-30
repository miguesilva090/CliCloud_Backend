using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.Tratamentos
{
  [Table("ConfiguracaoTratamentos", Schema = "Core")]
  public class ConfiguracaoTratamentos : AuditableEntity
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = default!;

    // Tipos de Serviço / Tratamentos
    [StringLength(50)]
    public string? TipoSrvTratamentos { get; set; }

    // Áreas de Prestação por Defeito
    [StringLength(50)]
    public string? AreaPrestacaoDefeitoAreaZ { get; set; }

    // Aparelhos
    public bool? ControlarAparelhos { get; set; }

    // Subsistemas de Saúde
    public int? Segundos { get; set; }
    public int? FaltasMax { get; set; }
    public int? FaltasConsecutivasMax { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Taxamoderadora { get; set; }

    public bool? CredencialExternaAdse { get; set; }

    // Configuração da forma de pagamento
    public int? TipoPagamento { get; set; }

    // Configurações (Tratamentos)
    public bool? AvisoInqueritoSessoesDiarias { get; set; }
  }
}

