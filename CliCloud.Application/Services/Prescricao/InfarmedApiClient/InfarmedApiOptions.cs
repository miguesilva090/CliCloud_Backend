namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient;

public class InfarmedApiOptions
{
    public const string SectionName = "InfarmedApi";

    public const string HttpClientName = "InfarmedApi";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;
} 