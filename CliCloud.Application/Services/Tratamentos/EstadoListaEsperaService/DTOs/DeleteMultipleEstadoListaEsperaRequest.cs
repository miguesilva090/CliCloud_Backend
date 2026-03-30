using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs
{
    public class DeleteMultipleEstadoListaEsperaRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
