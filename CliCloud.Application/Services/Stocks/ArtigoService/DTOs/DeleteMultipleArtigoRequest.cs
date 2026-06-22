using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class DeleteMultipleArtigoRequest : IDto 
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}