using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class ConfiguracaoSmsDTO : IDto 
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }

        public bool Ativo { get; set; }
        public int UsenditArpoone { get; set; }

        public string? Url { get; set; }
        public string? Loginapi { get; set; }
        public string? Passwordapi { get; set; }
        public string? Numapi { get; set; }
        public string? Remetente { get; set; }

        public string? ArpooneUrl { get; set; }
        public string? ArpooneSender { get; set; }
        public string? ArpooneApiKey { get; set; }
        public Guid? ArpooneOrganizationID { get; set; }

        public string? WebhookDeliveredUrl { get; set; }
        public string? WebhookNotDeliveredUrl { get; set; }
        public string? WebhookPendingUrl { get; set; }


        public DateTime? ControloSmsAutomaticos { get; set; }
    }
}