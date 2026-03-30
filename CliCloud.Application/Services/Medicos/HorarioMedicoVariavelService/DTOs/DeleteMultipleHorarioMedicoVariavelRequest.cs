namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs
{
    public class DeleteMultipleHorarioMedicoVariavelRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}
