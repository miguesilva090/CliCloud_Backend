using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Filters;

public class ListaEsperaTableFilter : PaginationFilter
{
    public List<TableFilter> Filters { get; set; } = [];

    public Guid? MedicoId { get; set; }

    /// <summary>Médico activo na agenda (filtro legado c_medico em ListaEsperaLst).</summary>
    public Guid? MedicoAgendaId { get; set; }

    /// <summary>Preenchido no serviço a partir de <see cref="MedicoAgendaId"/>.</summary>
    public Guid? MedicoAgendaEspecialidadeId { get; set; }

    public Guid? UtenteId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? PrioridadeId { get; set; }

    public DateTime? DataDe { get; set; }
    public DateTime? DataAte { get; set; }

    public bool IncluirConvertidos { get; set; }
}