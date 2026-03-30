using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Filters
{
    public class OdontogramaDefinitivoTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
        public Guid? ConsultaId { get; set; }
    }
}
