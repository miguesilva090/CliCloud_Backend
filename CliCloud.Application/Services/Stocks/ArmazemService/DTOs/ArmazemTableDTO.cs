using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArmazemService.DTOs;

public class ArmazemTableDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Localidade { get; set; }
    public string? Telefone { get; set; }
    public bool ArmazemGeral { get; set; }
    public DateTime CreatedOn { get; set; }
}
