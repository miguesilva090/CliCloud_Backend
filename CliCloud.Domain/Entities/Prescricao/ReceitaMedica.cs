#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Prescricao
{
    [Table("ReceitaMedica", Schema = "Prescricao")]
    public class ReceitaMedica : AuditableEntityWithSoftDelete
    {
        [Required] public Guid UtenteId { get; set; }
        [ForeignKey(nameof(UtenteId))] public Utente Utente { get; set; } = null!;

        [Required] public Guid MedicoId { get; set; }
        [ForeignKey(nameof(MedicoId))] public Medico Medico { get; set; } = null!;

        [Required] public Guid ClinicaId { get; set; }
        [ForeignKey(nameof(ClinicaId))] public Clinica Clinica { get; set; } = null!;

        [Required] public DateTime DataPrescricao { get; set; }

        public int TipoReceita { get; set; } = 1;

        public int Desmaterializada { get; set; } = 1;

        [StringLength(50)] public string? NumeroReceitaLocal { get; set; }
        [StringLength(50)] public string? NumeroReceita { get; set; }
        [StringLength(50)] public string? CodigoAcesso { get; set; }

        public int Enviada { get; set; } 
        public int Anulada { get; set; } 
        public DateTime? DataAnulacao { get; set; }
        [StringLength(20)] public string? MotivoAnulacaoCodigo { get; set; }
        [StringLength(500)] public string? MotivoAnulacaoDescricao { get; set; }

        public int ReceitaRenovavel { get; set; }
        public int? NumeroVias { get; set; }
        public int PrescricaoPorNome { get; set; }
        public int? MotivoPrescricaoNome { get; set; }

        [StringLength(50)] public string? NumeroBeneficiarioEfr { get; set; }
        [StringLength(20)] public string? SiglaEfr { get; set; }
        [StringLength(100)] public string? LocalPrescricao { get; set; }
        [StringLength(2000)] public string? Observacoes { get; set; }

        public int EstadoEnvio { get; set; }
        [StringLength(2000)] public string? MensagemErro { get; set; }

        public ICollection<ReceitaLinha> Linhas { get; set; } = new List<ReceitaLinha>();
    }
}