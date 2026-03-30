using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Alergias.AlergiaService.DTOs
{
    public class AlergiaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

