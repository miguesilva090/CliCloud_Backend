using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs
{
    public class GrupoSanguineoDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
