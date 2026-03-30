using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class WebhookEstadoSmsDTO
    {
        [JsonPropertyName("Msisdn")]
        public string? Msisdn { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("MessageId")]
        public string? MessageId { get; set; }

        [JsonPropertyName("OrganizationId")]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("ReportDescription")]
        public string? ReportDescription { get; set; }

        [JsonPropertyName("ReportDateTime")]
        public DateTime? ReportDateTime { get; set; }

        [JsonPropertyName("TotalSegments")]
        public int? TotalSegments { get; set; }

        [JsonPropertyName("CustomPayload")]
        public string? CustomPayload { get; set; }
    }
}