namespace CliCloud.Application.Services.Sexos.SexoService.DTOs
{
    public class DeleteMultipleSexoRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}

