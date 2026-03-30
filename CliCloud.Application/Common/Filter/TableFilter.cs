using System.Text.Json.Serialization;

namespace CliCloud.Application.Common.Filter
{
  public class TableFilter
  {
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("value")]
    public required string Value { get; set; }
  }
}
