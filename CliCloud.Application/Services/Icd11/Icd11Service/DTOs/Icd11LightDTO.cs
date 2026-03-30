using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Icd11Service.DTOs
{
    public class Icd11LightDTO : IDto
    {
        public int Id { get; set; }
        public string IcdId { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ClassKind { get; set; } = string.Empty;
        public int Level { get; set; }
        public int? ParentId { get; set; }
    }
}