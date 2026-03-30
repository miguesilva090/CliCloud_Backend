using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs
{
    public class FraquezasMuscularesLightDTO : IDto
    {
        public Guid Id {get;set;}
        public string? Descricao {get;set;}
    }
}