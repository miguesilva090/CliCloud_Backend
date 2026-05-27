#nullable enable

using System.ComponentModel.DataAnnotations;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class AnularDocumentoRequest : IDto 
{
    [Required]
    public required string MotivoAnulacao { get; set; }
    public DateTime? DataAnulacao { get; set; }
    public bool ReverterEstadosClinicos { get; set; } = true;
}