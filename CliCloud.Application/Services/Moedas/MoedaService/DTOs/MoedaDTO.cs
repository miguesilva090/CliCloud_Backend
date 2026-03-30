using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Moedas.MoedaService.DTOs
{
    public class MoedaDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Plural { get; set; }
        public decimal Cambio { get; set; }
        public string? Abreviatura { get; set; }
        public string? Centesimos { get; set; }
        public string? CentesimoPlural { get; set; }
        public string? Simbolo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
