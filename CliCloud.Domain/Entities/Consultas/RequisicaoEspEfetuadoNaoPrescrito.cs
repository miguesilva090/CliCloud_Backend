#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas;

[Table("RequisicaoEspEfetuadoNaoPrescrito", Schema = "Consultas")]
public class RequisicaoEspEfetuadoNaoPrescrito : AuditableEntityWithSoftDelete
{
    public int Codigo { get; set; }

    public Guid RequisicaoEspId { get; set; }

    public RequisicaoEsp RequisicaoEsp { get; set; } = null!;

    [StringLength(20)]
    public string CodigoMcdt { get; set; } = string.Empty;

    public int NAmostras { get; set; }
}
