using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;

namespace CliCloud.Application.Services.Utility.FreguesiaService.DTOs
{
    public class FreguesiaDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? ConcelhoId { get; set; }
        public ConcelhoDTO? Concelho { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

