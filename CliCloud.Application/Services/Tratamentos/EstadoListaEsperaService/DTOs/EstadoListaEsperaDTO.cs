using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs
{
    public class EstadoListaEsperaDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
