using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class GrupoViasAdministracaoLinhaItemDTO : IDto
    {
        public Guid? Id { get; set; }
        public Guid? ViaId { get; set; }
        public string? ViaDescricao { get; set; }
        public string? Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public int Linha { get; set; }
    }
}
