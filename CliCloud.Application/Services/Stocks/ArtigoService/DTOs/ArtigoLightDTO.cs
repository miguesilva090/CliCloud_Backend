using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class ArtigoLightDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string NumeroArtigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string AutocompleteLabel => $"{NumeroArtigo} - {Descricao}";
}