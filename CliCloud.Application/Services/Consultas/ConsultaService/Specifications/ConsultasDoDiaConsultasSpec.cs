using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications;

public sealed class ConsultasDoDiaConsultasSpec : Specification<Consulta>
{
  public ConsultasDoDiaConsultasSpec(DateTime data, Guid? medicoId, bool desmarcadas)
  {
    DateTime dia = data.Date;

    _ = Query
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.Organismo)
      .Include(x => x.TipoConsultaItem)
      .Include(x => x.TipoAdmissao)
      .Include(x => x.ConsultaMarcacao)
      .Where(x =>
        x.DeletedOn == null
        && x.Data.HasValue
        && x.Data.Value.Date == dia);

    if (medicoId.HasValue)
    {
      _ = Query.Where(x => x.MedicoId == medicoId.Value);
    }

    if (desmarcadas)
    {
      _ = Query.Where(x =>
        x.StatusConsulta == StatusConsulta.Desmarcada
        || x.StatusConsulta == StatusConsulta.Suspensa);
    }
    else
    {
      _ = Query.Where(x =>
        x.StatusConsulta == null
        || (
          x.StatusConsulta != StatusConsulta.Desmarcada
          && x.StatusConsulta != StatusConsulta.Suspensa));
    }

    _ = Query.OrderBy(x => x.HoraInicio).ThenBy(x => x.CreatedOn);
  }
}
