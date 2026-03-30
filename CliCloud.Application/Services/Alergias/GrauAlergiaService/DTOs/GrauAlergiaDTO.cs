using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrauAlergiaService.DTOs
{
    public class GrauAlergiaDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

