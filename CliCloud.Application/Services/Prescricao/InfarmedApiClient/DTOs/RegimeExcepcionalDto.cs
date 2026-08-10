using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class RegimeExcepcionalDto
{
    public int RegimeExcepcionalId { get; set; }
    public string? Descr { get; set; }
    public string? IndAtivo { get; set; }
}

public class InfarmedRefsPagedResponse<T>
{
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}
