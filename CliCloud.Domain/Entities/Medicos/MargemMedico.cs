#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Medicos
{
    [Table("MargemMedico", Schema = "Medicos")]
    public class MargemMedico : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid ServicoId { get; set; }

        [ForeignKey(nameof(ServicoId))]
        public Servico? Servico { get; set; }

        [Required]
        public Guid MedicoId { get; set; }

        [ForeignKey(nameof(MedicoId))]
        public Medico? Medico { get; set; }

        public decimal? ValorMargem { get; set; }

        public decimal? PercentagemMargem { get; set; }

    }
}