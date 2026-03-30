namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs
{
    public class DeleteMultipleGrauParentescoRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}
