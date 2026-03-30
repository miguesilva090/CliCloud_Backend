using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AnalisesService.DTOs
{
    public class AnaliseTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? UnidadeMedida { get; set; }
        public string? ValoresReferencia { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
