using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.ExameService.Specifications
{
  public class ExameByIdWithLinhas : Specification<Exame>
  {
    public ExameByIdWithLinhas(Guid id)
    {
      _ = Query.Where(x => x.Id == id)
        .Include(x => x.Linhas)
        .ThenInclude(l => l.TipoExame);
    }
  }
}
