#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public sealed class AtualizarValidacaoTransporteRequest : IDto
{
    public string CodigoValidacaoTransporte { get; set; } = string.Empty;
    public DateTime? DataTransporte { get; set; }
    public string? HoraTransporte { get; set; }
}
