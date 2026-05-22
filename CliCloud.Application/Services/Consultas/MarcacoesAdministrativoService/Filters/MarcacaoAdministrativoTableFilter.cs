using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Filters;

public class MarcacaoAdministrativoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];
    public DateTime? DataDe { get; set; }
    public DateTime? DataAte { get; set; }
    public TimeSpan? HoraDe { get; set; }
    public TimeSpan? HoraAte { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? UtenteId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? OrganismoId { get; set; }
    public bool ApenasAtivas { get; set; } = true;
}