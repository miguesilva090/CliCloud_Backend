using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.DTOs
{
    public class CodigoPostalLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public string? Localidade { get; set; }
   
    }
}

