#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.Sms
{
    [Table("HistoricoSms", Schema = "Core")]
    public class HistoricoSms : AuditableEntity
    {
        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        public Guid IdMensagem { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string TextoMensagem { get; set; } = string.Empty;

        [StringLength(30)]
        public string NumeroDestinatario { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = "Pendente";

        [Column(TypeName = "nvarchar(max)")]
        public string? MensagemErro { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public DateTime? DataHoraEnvio { get; set; }

        [StringLength(20)]
        public string Modulo { get; set; } = string.Empty;

        public int? CodigoUtente { get; set; }

        [StringLength(50)]
        public string? CodigoMedico { get; set; }

        public int? CodigoFisioterapeuta { get; set; }

        public int? CodigoConsulta { get; set; }

        public int? CodigoTratamento { get; set; }

        public int? CodigoAula { get; set; }
    }
}