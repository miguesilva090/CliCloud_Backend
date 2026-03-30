#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.Sms
{
    [Table("SmsRecebido", Schema = "Core")]
    public class SmsRecebido : AuditableEntity
    {
        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; } = null!;

        public Guid? OrganizacaoId { get; set; }

        public DateTime? DataHoraRecebimento { get; set; }

        [StringLength(30)]
        public string NumeroOrigem { get; set; } = string.Empty;

        [StringLength(30)]
        public string NumeroDestino { get; set; } = string.Empty;

        [StringLength(100)]
        public string Keyword { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string TextoMensagem { get; set; } = string.Empty;

        public int? Encoding { get; set; }

        [StringLength(10)]
        public string Mcc { get; set; } = string.Empty;

        [StringLength(10)]
        public string Mnc { get; set; } = string.Empty;

        public int? TotalSegmentos { get; set; }

        public DateTime DataHoraProcessamento { get; set; }

        public int? CodigoUtente { get; set; }
    }
}