using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs
{
    public class HabilitacaoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
