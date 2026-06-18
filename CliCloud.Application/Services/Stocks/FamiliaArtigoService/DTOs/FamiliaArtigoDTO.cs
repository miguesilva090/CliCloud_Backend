using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class FamiliaArtigoDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public Guid? ParentId { get; set; }
    public int Nivel { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? UrlFoto { get; set; }
    public string Path { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}