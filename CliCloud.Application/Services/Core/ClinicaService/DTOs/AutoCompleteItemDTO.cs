using System.Text.Json.Serialization;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class AutoCompleteItemDTO : IDto
  {
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;
  }
}

