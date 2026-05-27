#nullable enable

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class CriarNotaCreditoRequest : IDto
{
    [Required]
    public required Guid DocumentoOrigemId { get; set; }
    [Required]
    public required Guid TipoDocumentoId { get; set; }
    [Required]
    public required int AnoFiscal { get; set; }
    public DateTime? DataDocumento { get; set; }
    [Required]
    public required string Motivo { get; set; }
    public bool CreditoTotal { get; set; }
    public List<CriarNotaCreditoLinhaRequest> Linhas { get; set; } = [];
    public bool ReverterEstadosClinicos { get; set; } = true;
}