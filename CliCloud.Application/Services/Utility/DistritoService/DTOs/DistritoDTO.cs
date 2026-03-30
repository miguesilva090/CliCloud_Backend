using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.PaisService.DTOs;

namespace CliCloud.Application.Services.Utility.DistritoService.DTOs
{
    public class DistritoDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? PaisId { get; set; }
        public PaisDTO? Pais { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

