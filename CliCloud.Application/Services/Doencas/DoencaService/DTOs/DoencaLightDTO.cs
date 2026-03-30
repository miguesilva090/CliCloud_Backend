using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Doencas.DoencaService.DTOs
{
    public class DoencaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string IcdId { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClassKind { get; set; } = string.Empty;
        public int Level { get; set; }
        public Guid? ParentId { get; set; }
    }
}
