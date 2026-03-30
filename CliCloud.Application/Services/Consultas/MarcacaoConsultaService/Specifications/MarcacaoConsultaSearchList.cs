using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Specifications
{
  /// <summary>
  /// Pesquisa simples sobre ConsultaMarcacao, alinhada com a entidade actual.
  /// </summary>
  public class MarcacaoConsultaSearchList : Specification<ConsultaMarcacao>
  {
    public MarcacaoConsultaSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        if (Guid.TryParse(keyword, out var g))
        {
          _ = Query.Where(x =>
            x.Id == g
            || x.ConsultaId == g
            || x.UtenteId == g
            || x.MedicoId == g
            || x.EspecialidadeId == g
            || x.TecnicoId == g
            || x.FuncionarioId == g
            || x.MedicoExternoId == g
            || x.SalaId == g
            || x.MotivoConsultaId == g
            || x.TipoAdmissaoId == g
          );
        }
        else
        {
          _ = Query.Where(x =>
            (x.Obs != null && x.Obs.Contains(keyword))
            || (x.NumDestacavel != null && x.NumDestacavel.Contains(keyword))
          );
        }
      }

      _ = Query.OrderByDescending(x => x.Data);
    }
  }
}

