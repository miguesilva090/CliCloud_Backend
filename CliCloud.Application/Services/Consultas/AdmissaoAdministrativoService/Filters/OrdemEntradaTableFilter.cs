using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;

public class OrdemEntradaTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];

    public DateTime? DataDe { get; set; }
    public DateTime? DataAte { get; set; }
    public Guid? UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public bool IncluirHistorico { get; set; }
}