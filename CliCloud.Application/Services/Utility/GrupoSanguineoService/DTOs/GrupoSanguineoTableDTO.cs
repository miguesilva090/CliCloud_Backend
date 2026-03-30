using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs
{
    public class GrupoSanguineoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
