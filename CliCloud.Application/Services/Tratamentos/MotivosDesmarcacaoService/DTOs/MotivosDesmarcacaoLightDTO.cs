using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs
{
    public class MotivosDesmarcacaoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}