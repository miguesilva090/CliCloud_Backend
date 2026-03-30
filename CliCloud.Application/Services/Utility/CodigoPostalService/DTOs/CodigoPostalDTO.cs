using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.DTOs
{
    public class CodigoPostalDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public string? Localidade { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

