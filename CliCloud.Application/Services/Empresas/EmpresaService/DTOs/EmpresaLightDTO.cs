using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Empresas.EmpresaService.DTOs
{
    public class EmpresaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
    }
}
