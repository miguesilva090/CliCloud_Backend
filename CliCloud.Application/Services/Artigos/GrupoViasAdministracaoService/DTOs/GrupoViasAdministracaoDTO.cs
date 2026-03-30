using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class GrupoViasAdministracaoDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public List<GrupoViasAdministracaoLinhaItemDTO> Vias { get; set; } = [];
    }
}
