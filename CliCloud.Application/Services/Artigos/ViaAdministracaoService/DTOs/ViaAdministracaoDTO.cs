using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs
{
    public class ViaAdministracaoDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
