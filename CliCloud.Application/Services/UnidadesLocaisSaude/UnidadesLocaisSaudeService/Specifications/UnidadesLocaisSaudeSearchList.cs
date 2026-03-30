using Ardalis.Specification;
using UnidadesLocaisSaudeEntity = CliCloud.Domain.Entities.UnidadesLocaisSaude.UnidadesLocaisSaude;
using System.Globalization;

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
          x.Codigo.ToString(CultureInfo.InvariantCulture).Contains(keyword)
        );
      }

      _ = Query.OrderByDescending(x => x.CreatedOn);
    }
  }
}

