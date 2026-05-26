using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Specifications;

public sealed class MarcacaoAdministrativoCalendarioSpec : Specification<ConsultaMarcacao>
{
  public MarcacaoAdministrativoCalendarioSpec(
    Guid medicoId,
    DateTime dataDe,
    DateTime dataAte,
    Guid? salaId,
    Guid? especialidadeId
  )
  {
    DateTime de = dataDe.Date;
    DateTime ate = dataAte.Date;

    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Sala)
      .Include(x => x.TipoConsultaItem)
      .Where(x =>
        x.DeletedOn == null
        &&
        x.MedicoId == medicoId
        && x.Data.HasValue
        && x.Data.Value.Date >= de
        && x.Data.Value.Date <= ate
        && (
          x.StatusConsulta == null
          || (
            x.StatusConsulta != StatusConsulta.Desmarcada
            && x.StatusConsulta != StatusConsulta.Suspensa
            && x.StatusConsulta != StatusConsulta.Concluida
          )
        )
      );

    if (especialidadeId.HasValue)
    {
      _ = Query.Where(x => x.EspecialidadeId == especialidadeId.Value);
    }

    if (salaId.HasValue)
    {
      _ = Query.Where(x => x.SalaId == salaId.Value);
    }

    _ = Query.OrderBy(x => x.Data).ThenBy(x => x.HoraMarcacao);
  }
}
