using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs
{
    public class DeleteMultipleMotivoRetencaoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
