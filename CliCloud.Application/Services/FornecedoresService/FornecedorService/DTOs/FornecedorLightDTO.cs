using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.DTOs
{
    public class FornecedorLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public string? RuaNome { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }
        public string? CodigoPostalLocalidade { get; set; }
        public Guid? FreguesiaId { get; set; }
        public string? FreguesiaNome { get; set; }
        public Guid? ConcelhoId { get; set; }
        public string? ConcelhoNome { get; set; }
        public Guid? DistritoId { get; set; }
        public string? DistritoNome { get; set; }
        public Guid? PaisId { get; set; }
        public string? PaisNome { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public int? Status { get; set; }
        public OrigemFornecedor? Origem { get; set; }
        public TipoFornecedor? TipoFornecedor { get; set; }
    }
}
