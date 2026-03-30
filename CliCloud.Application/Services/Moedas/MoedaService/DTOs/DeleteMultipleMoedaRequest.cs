using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Moedas.MoedaService.DTOs
{
    public class DeleteMultipleMoedaRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}
