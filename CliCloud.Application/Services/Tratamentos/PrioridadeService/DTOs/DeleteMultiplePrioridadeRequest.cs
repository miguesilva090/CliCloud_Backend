using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs
{
    public class DeleteMultiplePrioridadeRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
