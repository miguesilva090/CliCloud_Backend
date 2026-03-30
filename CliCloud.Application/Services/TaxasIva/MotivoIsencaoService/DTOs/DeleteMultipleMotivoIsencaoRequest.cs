using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class DeleteMultipleMotivoIsencaoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
