using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class TipoExameTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string? EAN { get; set; }
        public bool Inativo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
