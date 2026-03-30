using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AcordosService.DTOs
{
    public class DeleteMultipleAcordosRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
