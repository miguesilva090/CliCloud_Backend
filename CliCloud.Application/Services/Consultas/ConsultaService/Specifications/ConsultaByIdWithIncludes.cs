using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications
{
  public class ConsultaByIdWithIncludes : Specification<Consulta>
  {
    public ConsultaByIdWithIncludes(Guid id)
    {
      _ = Query.Where(x => x.Id == id)
        .Include(x => x.TipoConsultaItem);
    }
  }
}
