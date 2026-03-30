using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class GrupoViasAdministracaoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}

