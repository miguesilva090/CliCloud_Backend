#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Domain.Entities.Exames
{
    /// <summary>
    /// Acordo entre Tipo de Exame e Organismo (subsistema de saúde).
    /// No projeto novo os "subsistemas de saúde" são os Organismos (ADSE, ARS, etc.) — ligação via OrganismoId.
    /// Corresponde ao ecrã legado "Acordos": Cód. Subsistema (texto), Cód. Organismo, Cód. Tipo Exame, valores e margem.
    /// </summary>
    [Table("Acordos", Schema = "Exames")]
    public class Acordos : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid TipoExameId { get; set; }
        [ForeignKey("TipoExameId")]
        public TipoExame TipoExame { get; set; } = null!;

        [Required]
        public Guid OrganismoId { get; set; }
        [ForeignKey("OrganismoId")]
        public Organismo Organismo { get; set; } = null!;

        [StringLength(20)]
        public string? CodigoSubsistema { get; set; }

        public decimal? ValTipoExame { get; set; }

        public decimal? ValorOrganismo { get; set; }

        public decimal? MargemOrganismo { get; set; }

        public decimal? ValorUtente { get; set; }

        public bool Inactivo { get; set; }
    }
}
