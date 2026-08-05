using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.Specifications;

public sealed class SessoesPlanningTecnicoPeriodoSpec : Specification<SessaoTratamento>
{
    public SessoesPlanningTecnicoPeriodoSpec(
        Guid tecnicoId,
        TipoTecnico tipoTecnico,
        DateTime dataDe,
        DateTime dataAte
    )
    {
        DateTime ini = dataDe.Date;
        DateTime fimExclusivo = dataAte.Date.AddDays(1);

        _ = Query
            .Include(x => x.Tratamento!)
                .ThenInclude(t => t.Utente)
            .Where(x => 
                x.DeletedOn == null
                && x.Data != null
                && x.Data >= ini
                && x.Data < fimExclusivo
                && (x.Desmarcado == null || x.Desmarcado == 0));

        _ = tipoTecnico switch
        {
            TipoTecnico.Auxiliar => Query.Where(x => x.AuxiliarId == tecnicoId),
            TipoTecnico.Outro => Query.Where(x => x.OutroTecnicoId == tecnicoId),
            _ => Query.Where(x => x.FisioterapeutaId == tecnicoId),
        };

        _ = Query.OrderBy(x => x.Data).ThenBy(x => x.HoraInic);
    }
}