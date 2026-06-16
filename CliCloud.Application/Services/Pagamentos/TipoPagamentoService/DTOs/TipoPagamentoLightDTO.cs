using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Pagamentos.TipoPagamentoService.DTOs;

public class TipoPagamentoLightDTO : IDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string AutocompleteLabel => $"{Descricao} ({Codigo})";
}
