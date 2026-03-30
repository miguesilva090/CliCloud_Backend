using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs
{
    public class LocalTratamentoDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
    }
}
