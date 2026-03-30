using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs
{
    public class ProfissaoDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
