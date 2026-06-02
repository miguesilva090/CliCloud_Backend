#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public sealed class DocumentoLiquidacaoContextoDTO : IDto 
{
    public Guid DocumentoId { get; set; }
    public bool JaLiquidado { get; set; }
    public bool IsUtente { get; set; }
    public Guid? UtenteId { get; set; }
    public Guid? OrganismoId { get; set; }
    public decimal TotalLiquido { get; set; }
    public string NumeroExibicao { get; set; } = string.Empty;
}