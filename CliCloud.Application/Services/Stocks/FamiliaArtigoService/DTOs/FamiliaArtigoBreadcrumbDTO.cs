using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class FamiliaArtigoBreadcrumbDTO : IDto
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Nivel { get; set; }
}
