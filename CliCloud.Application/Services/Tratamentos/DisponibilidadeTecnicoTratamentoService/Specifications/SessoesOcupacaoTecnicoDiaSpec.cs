using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.Specifications;

public sealed class SessoesOcupacaoTecnicoDiaSpec : Specification<SessaoTratamento>
{
    public SessoesOcupacaoTecnicoDiaSpec(
        Guid tecnicoId,
        TipoTecnico tipoTecnico,
        DateTime data,
        Guid? ignorarSessaoId 
    )
    {
        DateTime diaIni = data.Date;
        DateTime diaFim = diaIni.AddDays(1);

        _ = Query
            .Include(x => x.Tratamento)
            .Where(x => 
                x.DeletedOn == null
                && x.Data != null
                && x.Data >= diaIni
                && x.Data < diaFim
                && (x.Desmarcado == null || x.Desmarcado == 0));

        _ = tipoTecnico switch
        {
            TipoTecnico.Auxiliar => Query.Where(x => x.AuxiliarId == tecnicoId),
            TipoTecnico.Outro => Query.Where(x => x.OutroTecnicoId == tecnicoId),
            _ => Query.Where(x => x.FisioterapeutaId == tecnicoId),
        };

        if (ignorarSessaoId.HasValue)
        {
            Guid ignorar = ignorarSessaoId.Value;
            _ = Query.Where(x => x.Id != ignorar);
        }
    }
}