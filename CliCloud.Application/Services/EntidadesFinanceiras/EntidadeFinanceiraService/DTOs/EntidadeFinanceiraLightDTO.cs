using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs
{
    public class EntidadeFinanceiraLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Abreviatura { get; set; }
        public string PaisPrefixo { get; set; } = string.Empty;
        public string? TipoEntidadeFinanceiraDesignacao { get; set; }
        public CondicaoSns? CondicaoSns { get; set; }
    }
}
