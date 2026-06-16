using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;

public class ModoPagamentoLightDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public bool TemNumAssociado { get; set; }
    public bool TemContaBancaria { get; set; }
    public Guid? ContaBancariaId { get; set; }
    public string? ContaBancariaNumero { get; set; }
    public string AutocompleteLabel => $"{Descricao} ({Abreviatura})";
}
