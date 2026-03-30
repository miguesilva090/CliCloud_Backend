using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AnalisesService.DTOs
{
    public class DeleteMultipleAnaliseRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
