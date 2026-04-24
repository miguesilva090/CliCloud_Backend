#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tecnicos
{
    [Table("FolgasTecnico", Schema = "Tecnicos")]
    public class FolgasTecnico : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid TecnicoId { get; set; }

        [ForeignKey(nameof(TecnicoId))]
        public Tecnico Tecnico { get; set; } = null!;

        [Required]
        public DateTime DataDe { get; set; }

        [Required]
        public DateTime DataAte { get; set; }

        public bool TodoDia { get; set; }
        public bool MesInteiro { get; set; }

        public TimeSpan? ManhaInicio { get; set; }
        public TimeSpan? ManhaFim { get; set; }
        public TimeSpan? TardeInicio { get; set; }
        public TimeSpan? TardeFim { get; set; }
    }
}