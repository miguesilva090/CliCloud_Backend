using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs
{
    public class TipoDeDorLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
    }
}