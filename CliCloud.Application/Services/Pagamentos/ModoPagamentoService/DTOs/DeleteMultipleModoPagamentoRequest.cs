using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;

public class DeleteMultipleModoPagamentoRequest : IDto
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}
