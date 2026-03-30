using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.Sms
{
    [Table("ConfiguracaoSms" , Schema = "Core")]
    public class ConfiguracaoSms : AuditableEntity 
    {
        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        public bool Ativo { get; set; }

        public int UsenditArpoone { get; set; } = 1;

        [StringLength(500)]
        public string? Url { get; set; }

        [StringLength(500)]
        public string? Loginapi { get; set; } 

        [StringLength(500)]
        public string? Passwordapi { get; set; }

        [StringLength(100)]
        public string? Numapi { get; set; }

        [StringLength(100)]
        public string? Remetente { get; set; }

        [StringLength(500)]
        public string? ArpooneUrl { get; set; }

        [StringLength(100)]
        public string? ArpooneSender { get; set; }

        [StringLength(1000)]
        public string? ArpooneApiKey { get; set; }

        public Guid? ArpooneOrganizationID { get; set; }

        [StringLength(1000)]
        public string? WebhookDeliveredUrl { get; set; }

        [StringLength(1000)]
        public string? WebhookNotDeliveredUrl { get; set; } 

        [StringLength(1000)]
        public string? WebhookPendingUrl { get; set; }

        public DateTime? ControloSmsAutomaticos { get; set; }
    }
}