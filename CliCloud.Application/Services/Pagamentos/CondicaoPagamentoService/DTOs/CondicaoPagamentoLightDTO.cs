using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;

public class CondicaoPagamentoLightDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int? NDiasPagamento { get; set; }
    public decimal? Desconto { get; set; }
}
