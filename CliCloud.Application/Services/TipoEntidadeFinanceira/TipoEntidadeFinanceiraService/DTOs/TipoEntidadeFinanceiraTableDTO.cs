using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs
{
    public class TipoEntidadeFinanceiraTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Sigla { get; set; }
        public string? Designacao { get; set; }
        public string? Dominio { get; set; }
        public string? DescricaoDominio { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
