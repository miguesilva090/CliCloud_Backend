using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;

public class DeleteMultipleUnidadeMedidaRequest : IDto
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}
