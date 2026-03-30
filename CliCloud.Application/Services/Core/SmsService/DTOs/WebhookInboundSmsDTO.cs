using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class WebhookInboundSmsDTO
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("OrganizationId")]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("ReceiveDateTime")]
        public DateTime? ReceiveDateTime { get; set; }

        [JsonPropertyName("From")]
        public string? From { get; set; }

        [JsonPropertyName("To")]
        public string? To { get; set; }

        [JsonPropertyName("Keyword")]
        public string? Keyword { get; set; }

        [JsonPropertyName("Text")]
        public string? Text { get; set; }

        [JsonPropertyName("Encoding")]
        public int? Encoding { get; set; }

        [JsonPropertyName("Mcc")]
        public string? Mcc { get; set; }

        [JsonPropertyName("Mnc")]
        public string? Mnc { get; set; }

        [JsonPropertyName("TotalSegments")]
        public int? TotalSegments { get; set; }
    }
}