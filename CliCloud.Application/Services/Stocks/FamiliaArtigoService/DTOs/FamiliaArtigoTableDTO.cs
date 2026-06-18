using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class FamiliaArtigoTableDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public int Nivel { get; set; }

    public string Descricao { get; set; } = string.Empty;
    public bool TemFilhos { get; set; }
    public DateTime CreatedOn { get; set; }

}