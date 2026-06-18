using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Stocks.ArmazemService.DTOs;

public class ArmazemDTO : IDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Morada { get; set; }
    public string? Localidade { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public string? CodigoPostalCodigo { get; set; }
    public string? CodigoPostalLocalidade { get; set; }
    public string? Telefone { get; set; }
    public string? Fax { get; set; }
    public bool ArmazemGeral { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
