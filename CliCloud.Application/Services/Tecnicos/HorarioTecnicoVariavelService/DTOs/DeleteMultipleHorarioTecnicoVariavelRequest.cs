namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs
{
    public class DeleteMultipleHorarioTecnicoVariavelRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}