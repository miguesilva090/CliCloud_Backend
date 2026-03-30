using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs
{
    public class RegiaoCorpoDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

