using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class FamiliaArtigoLightDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string AutocompleteLabel => Path;
}