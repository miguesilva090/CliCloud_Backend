using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs
{
    public class ViaAdministracaoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}