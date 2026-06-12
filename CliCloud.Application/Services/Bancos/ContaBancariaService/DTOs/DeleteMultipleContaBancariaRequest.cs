using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs
{
    public class DeleteMultipleContaBancariaRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}