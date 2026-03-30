using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;

namespace CliCloud.Application.Services.Utility.ConcelhoService.DTOs
{
    public class ConcelhoDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? DistritoId { get; set; }
        public DistritoDTO? Distrito { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

