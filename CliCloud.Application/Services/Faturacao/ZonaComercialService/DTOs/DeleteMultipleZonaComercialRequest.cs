using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;

public class DeleteMultipleZonaComercialRequest : IDto 
{
    public IEnumerable<Guid> Ids { get; set; } = [];
}