using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArmazemService.DTOs;

public class ArmazemLightDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool ArmazemGeral { get; set; }
    public string AutocompleteLabel => $"{Codigo} - {Nome}";
}
