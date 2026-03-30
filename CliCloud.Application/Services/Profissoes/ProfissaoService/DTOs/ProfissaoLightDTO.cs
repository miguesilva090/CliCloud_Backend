using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs
{
    public class ProfissaoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
