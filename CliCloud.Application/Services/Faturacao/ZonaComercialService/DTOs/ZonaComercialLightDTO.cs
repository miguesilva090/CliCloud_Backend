using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;

public class ZonaComercialLightDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string AutocompleteLabel => $"{Codigo} - {Descricao}";
}