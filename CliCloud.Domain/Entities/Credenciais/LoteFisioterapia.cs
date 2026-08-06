#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Credenciais;

[Table("LoteFisioterapia", Schema = "Credenciais")]
public class LoteFisioterapia : AuditableEntityWithSoftDelete
{
    public int Indice { get; set; }
    public int NumeroLote { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CodigoOrganismo { get; set; }
    public int TipoLote { get; set; }
    public int TipoServico { get; set; }
    public DateTime DataLote { get; set; }
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTaxa { get; set; }

    [StringLength(1)]
    public string? Tipo { get; set; }

    public int? Isencao { get; set; }
    public int NumeroRequisicoes { get; set; }
    public int? TotalK { get; set; }
    public int? TotalC { get; set; }
}
