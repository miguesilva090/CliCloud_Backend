using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class DeleteMultipleTipoExameRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
