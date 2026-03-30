using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrauAlergiaService.DTOs
{
    public class GrauAlergiaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
