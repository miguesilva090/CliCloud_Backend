using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Specifications
{
    /// <summary>
    /// Lista de relatórios/atestados filtrados por utente, ordenados da data mais recente para a mais antiga.
    /// </summary>
    public sealed class RelatorioAtestadoByUtenteSpec : Specification<RelatorioAtestado>
    {
        public RelatorioAtestadoByUtenteSpec(Guid utenteId)
        {
            Query
                .Where(x => x.UtenteId == utenteId)
                .Include(x => x.Medico)
                .OrderByDescending(x => x.CreatedOn);
        }
    }
}

