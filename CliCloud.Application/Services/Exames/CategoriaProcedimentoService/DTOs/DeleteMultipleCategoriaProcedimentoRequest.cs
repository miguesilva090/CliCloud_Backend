using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs
{
    public class DeleteMultipleCategoriaProcedimentoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}
