namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs
{
    public class DeleteMultipleEstadoCivilRequest
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
