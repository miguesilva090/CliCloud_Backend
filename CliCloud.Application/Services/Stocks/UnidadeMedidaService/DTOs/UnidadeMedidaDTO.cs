using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;

public class UnidadeMedidaDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
