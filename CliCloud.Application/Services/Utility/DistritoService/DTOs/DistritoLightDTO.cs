using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.DistritoService.DTOs
{
    public class DistritoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? PaisId { get; set; }
        public string? PaisNome { get; set; }
       
    }
}

