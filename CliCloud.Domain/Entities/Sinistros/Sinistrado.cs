#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Sinistros
{
    [Table("Sinistrado", Schema = "Consultas")]
    public class Sinistrado : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string CodigoSinistro { get; set; } = string.Empty;
        public DateTime? DataAcidente { get; set; }
        public DateTime? DataParticipacao { get; set; }
        public DateTime? DataPrimeiraObservacao { get; set; }
        public DateTime? DataUltimoTratamento { get; set; }
        public DateTime? DataAlta { get; set; }

        [Required]
        public Guid UtenteId { get; set; }
        public Guid? EstadoSinistroId { get; set; }

        [StringLength(80)]
        public string? TipoAcidente { get; set; }

        [StringLength(80)]
        public string? Responsabilidade { get; set; }

        [StringLength(120)]
        public string? Diagnostico { get; set; }

        [StringLength(120)]
        public string? NumeroProcesso { get; set; }
        public bool Historico { get; set; }
        public string? Observacoes { get; set; }
        public string? Relatorio { get; set; }
        public EstadoSinistroItem? EstadoSinistro { get; set; }
        public ICollection<SinistradoLinhaServico> LinhasServico { get; set; } = new List<SinistradoLinhaServico>();
    }
}