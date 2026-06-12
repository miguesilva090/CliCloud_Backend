using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class MotivoIsencaoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? CodigoSaft { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Norma { get; set; }
        public string? Mencao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
