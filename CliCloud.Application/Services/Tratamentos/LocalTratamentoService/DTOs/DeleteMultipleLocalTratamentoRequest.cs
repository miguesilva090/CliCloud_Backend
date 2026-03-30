namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs
{
    public class DeleteMultipleLocalTratamentoRequest
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
