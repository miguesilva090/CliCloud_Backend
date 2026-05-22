using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissoesParaFechoSpec : Specification<Admissao>
{
  public AdmissoesParaFechoSpec(DateTime data, Guid clinicaId, bool incluirSemSala = true)
  {
    DateTime dia = data.Date;

    _ = Query
      .Where(x => x.DeletedOn == null)
      .Where(x => x.Data.HasValue && x.Data.Value.Date == dia)
      .Where(x =>
        x.StatusConsulta == null
        || (x.StatusConsulta != StatusConsulta.Desmarcada
            && x.StatusConsulta != StatusConsulta.Suspensa))
      .Include(x => x.Servicos)
      .Include(x => x.TipoAdmissao)
      .Include(x => x.Sala);

    if (incluirSemSala)
    {
      _ = Query.Where(x => x.SalaId == null || x.Sala!.ClinicaId == clinicaId);
    }
    else
    {
      _ = Query.Where(x => x.SalaId != null && x.Sala!.ClinicaId == clinicaId);
    }
  }
}
