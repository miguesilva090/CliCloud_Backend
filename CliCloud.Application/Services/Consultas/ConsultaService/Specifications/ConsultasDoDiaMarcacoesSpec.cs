using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications;

public sealed class ConsultasDoDiaMarcacoesSpec : Specification<ConsultaMarcacao>
{
  public ConsultasDoDiaMarcacoesSpec(DateTime data, Guid? medicoId, bool desmarcadas)
  {
    DateTime dia = data.Date;

    _ = Query
      .Include(x => x.Utente)
        .ThenInclude(u => u.Organismo)
      .Include(x => x.Medico)
      .Include(x => x.Especialidade)
      .Include(x => x.TipoConsultaItem)
      .Include(x => x.TipoAdmissao)
      .Include(x => x.Consulta)
      .Where(x =>
        x.DeletedOn == null
        && x.Data.HasValue
        && x.Data.Value.Date == dia);

    if (medicoId.HasValue)
    {
      _ = Query.Where(x => x.MedicoId == null || x.MedicoId == medicoId.Value);
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
          && x.StatusConsulta != StatusConsulta.Suspensa
          && x.StatusConsulta != StatusConsulta.Concluida));
    }

    _ = Query.OrderBy(x => x.HoraMarcacao).ThenBy(x => x.CreatedOn);
  }
}
