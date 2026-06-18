using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArmazemService.DTOs;

public class DeleteMultipleArmazemRequest : IDto
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}
