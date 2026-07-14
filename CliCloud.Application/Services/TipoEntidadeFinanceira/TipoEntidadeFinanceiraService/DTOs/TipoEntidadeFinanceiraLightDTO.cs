using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs
{
    public class TipoEntidadeFinanceiraLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Designacao { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
    }
}
