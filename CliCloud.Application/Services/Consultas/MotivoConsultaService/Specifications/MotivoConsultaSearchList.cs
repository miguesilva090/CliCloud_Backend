using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.Specifications
{
  public class MotivoConsultaSearchList : Specification<MotivoConsulta>
  {
    public MotivoConsultaSearchList(string keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x => x.Designacao.Contains(keyword));
      }

      _ = Query.OrderBy(x => x.Designacao);
    }
  }
}
