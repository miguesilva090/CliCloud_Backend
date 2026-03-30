using Ardalis.Specification;
using UnidadesLocaisSaudeEntity = CliCloud.Domain.Entities.UnidadesLocaisSaude.UnidadesLocaisSaude;

namespace CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.Specifications
{
  public class UnidadesLocaisSaudeSearchList : Specification<UnidadesLocaisSaudeEntity>
  {
    public UnidadesLocaisSaudeSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = Query.Where(x =>
          x.Nome.Contains(keyword) ||
          (x.Nif != null && x.Nif.Contains(keyword)) ||
          x.Codigo.ToString().Contains(keyword)
        );
      }

      _ = Query.OrderByDescending(x => x.CreatedOn);
    }
  }
}

