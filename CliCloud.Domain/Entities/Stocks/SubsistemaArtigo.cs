#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Domain.Entities.Stocks;

[Table("SubsistemaArtigo", Schema = "Stocks")]
public class SubsistemaArtigo : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }
    
    public Guid ClinicaId { get; set; }

    public Guid ArtigoId { get; set; }
    public Artigo Artigo { get; set; } = null!;

    public Guid OrganismoId { get; set; }
    public Organismo Organismo { get; set; } = null!;

    [StringLength(20)]
    public string CodigoCartaoInstituicao { get; set; } = string.Empty;

    public decimal ValorServico { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal ValorUtente { get; set; } 

    public bool Inativo { get; set; }

    [StringLength(10)]
    public string? CodigoComplementarAdse { get; set; }

}