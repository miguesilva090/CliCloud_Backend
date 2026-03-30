using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  public class ClinicaSearchList : Specification<Clinica>
  {
    public ClinicaSearchList(string? keyword = "")
    {
      if (!string.IsNullOrWhiteSpace(keyword))
        _ = Query.Where(x =>
          x.Nome.Contains(keyword) ||
          (x.NomeComercial != null && x.NomeComercial.Contains(keyword)) ||
          (x.Abreviatura != null && x.Abreviatura.Contains(keyword)));
      _ = Query.OrderBy(x => x.Nome);
    }
  }
}
