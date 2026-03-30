using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;


namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications
{
  public class ConsultaSearchList : Specification<Consulta>
  {
    public ConsultaSearchList(string? keyword = "")
    {
      _ = Query
        .Include(x => x.TipoConsultaItem)
        .Include(x => x.Sala)
        .Include(x => x.Utente);

      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.UtenteId == g
            || x.MedicoId == g
            || x.EspecialidadeId == g
            || x.TecnicoId == g
            || x.DocumentoId == g
            || x.TipoDocumentoId == g
            || x.OrganismoId == g
            || x.SeguradoraId == g
            || x.TratamentoId == g
            || x.FuncionarioId == g
            || x.TipoConsultaId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.Sala != null && x.Sala.Nome.Contains(keyword))
            || (x.Utente != null && x.Utente.Nome.Contains(keyword))
            || (x.HoraInicio.HasValue && x.HoraInicio.Value.ToString(@"hh\:mm").Contains(keyword))
            || (x.HoraFim.HasValue && x.HoraFim.Value.ToString(@"hh\:mm").Contains(keyword))
            || (x.Diagnostico != null && x.Diagnostico.Contains(keyword))
            || (x.Obs != null && x.Obs.Contains(keyword))
            || (x.Credencial != null && x.Credencial.Contains(keyword))
            || (x.TipoConsultaItem != null && x.TipoConsultaItem.Designacao.Contains(keyword))
          );
        }
      }

      _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
    }
  }
}
