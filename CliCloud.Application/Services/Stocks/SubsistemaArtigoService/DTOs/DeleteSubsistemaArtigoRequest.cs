using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;

public class DeleteMultipleSubsistemaArtigoRequest : IDto
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}