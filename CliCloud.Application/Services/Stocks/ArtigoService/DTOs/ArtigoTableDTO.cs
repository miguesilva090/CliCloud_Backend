using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class ArtigoTableDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string NumeroArtigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? ArmazemNome { get; set; }
    public decimal PrecoUnitarioSemIva1 { get; set; }
    public decimal PrecoVendaComIva1 { get; set; }
    public bool Inativo { get; set; }
    public bool Descontinuado { get; set; }
    public TipoArtigoStocks TipoArtigo { get; set; }
    public DateTime CreatedOn { get; set; }
}