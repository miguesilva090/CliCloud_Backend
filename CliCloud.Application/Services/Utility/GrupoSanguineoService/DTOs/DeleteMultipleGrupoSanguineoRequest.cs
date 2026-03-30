namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs
{
    public class DeleteMultipleGrupoSanguineoRequest
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
