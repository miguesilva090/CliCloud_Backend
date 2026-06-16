using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;

public class DeleteMultipleCondicaoPagamentoRequest : IDto
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}
