using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs
{
    public class HabilitacaoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
