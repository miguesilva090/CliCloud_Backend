#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

public class SinistradosInfoFaturacaoRequest : IDto 
{
    public Guid SinistradoId { get; set; }
    public DateTime? DataDesde { get; set; }
    public DateTime? DataAte { get; set; }
    /// <summary>Legado filtroSinistrado — clínica de origem dos serviços. Opcional; usa a clínica da sessão.</summary>
    public Guid? ClinicaOrigemServicosId { get; set; }
}