#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Stocks;

[Table("FamiliaArtigo", Schema = "Stocks")]
public class FamiliaArtigo : AuditableEntityWithSoftDelete
{
    [Key] 
    public new Guid Id { get; set; }

    public Guid ClinicaId { get;set; }

    public int Codigo { get; set; }

    public Guid? ParentId { get; set; }
    public FamiliaArtigo? Parent { get; set; }

    public int Nivel { get; set; }

    [Required]
    [StringLength(50)]
    public string Descricao { get; set; } = string.Empty;

    [StringLength(512)]
    public string? UrlFoto { get; set; }

    public ICollection<FamiliaArtigo> Children { get; set; } = [];
}