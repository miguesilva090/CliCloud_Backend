using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class IfThenCallbackRequest : IDto 
{
    public string Key { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;

    public string? Entity { get; set; }
    public string? Reference { get; set; }
    public string? PaymentDateTime { get; set; }
    public string? OrderId { get; set; }
    public decimal? Amount { get; set; }
}