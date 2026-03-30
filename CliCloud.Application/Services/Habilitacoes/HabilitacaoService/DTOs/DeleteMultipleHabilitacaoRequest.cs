namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs
{
    public class DeleteMultipleHabilitacaoRequest
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
