#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados
{
    [Table("FichaClinicaSecaoConteudo", Schema = "ProcessoClinico")]
    public class FichaClinicaSecaoConteudo : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public Guid CampoId { get; set; }
        public FichaClinicaSecaoCampo Campo { get; set; } = null!;

        [Required]
        public string Texto { get; set; } = string.Empty;
    }
}