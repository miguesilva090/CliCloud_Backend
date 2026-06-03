#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class FaturaGlobalObterRequest : IDto 
{
    public Guid OrganismoId { get; set; }
    public string TipoFatura { get; set; } = string.Empty;
    public DateTime DataDe { get; set; }
    public DateTime DataAte { get; set; }
    public int? OpcaoTipo { get; set; }
    public Guid? UtenteId { get; set; }
    public string? CodigoRequisicao { get; set; }
    public string? NumeroProcesso { get; set; }
    
}