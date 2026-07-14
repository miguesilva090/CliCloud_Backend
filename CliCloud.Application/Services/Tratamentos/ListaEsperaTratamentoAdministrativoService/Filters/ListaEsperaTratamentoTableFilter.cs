using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Filters;

public class ListaEsperaTratamentoTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];

    /// <summary>false = lista activa; true = histórico (legado historico_de/ate).</summary>
    public bool Historico { get; set; }

    public Guid? UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? PrioridadeId { get; set; }
    public Guid? LocalTratamentoId { get; set; }
    public Guid? EstadoListaEsperaId { get; set; }
}
