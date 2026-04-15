using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;

public class SeparadorDTO : IDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
