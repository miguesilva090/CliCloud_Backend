#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tecnicos
{
    [Table("HorarioTecnicoVariavel", Schema = "Tecnicos")]
    public class HorarioTecnicoVariavel : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid TecnicoId { get; set; }

        [ForeignKey(nameof(TecnicoId))]
        public Tecnico Tecnico { get; set; } = null!;

        [Required]
        public DateTime Data { get; set; }

        public TimeSpan? ManhaInicio { get; set; }
        public TimeSpan? ManhaFim { get; set; }
        public TimeSpan? TardeInicio { get; set; }
        public TimeSpan? TardeFim { get; set; }
    }
}