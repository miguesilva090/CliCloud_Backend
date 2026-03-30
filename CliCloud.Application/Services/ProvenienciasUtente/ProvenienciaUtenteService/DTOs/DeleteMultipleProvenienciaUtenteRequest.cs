namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs
{
    public class DeleteMultipleProvenienciaUtenteRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}
