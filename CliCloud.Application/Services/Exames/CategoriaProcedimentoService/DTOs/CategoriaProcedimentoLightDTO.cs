using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs
{
    public class CategoriaProcedimentoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
