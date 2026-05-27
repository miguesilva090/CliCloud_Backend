#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class DocumentoEmissaoDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid TipoDocumentoId { get; set; }

    public int AnoFiscal { get; set; }
    public int NumeroDocumento { get; set; }

    public string? NumeroExibicao { get; set; }
    public string? HashDocumento { get; set; }
    public int? VersaoChave { get; set; }
    
}