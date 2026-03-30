using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  public class ClinicaAutocompleteSearchList : Specification<Clinica>
  {
    public ClinicaAutocompleteSearchList(string? keyword = null)
    {
      if (!string.IsNullOrWhiteSpace(keyword))
      {
        _ = keyword.Trim();
        _ = Query.Where(c =>
          (c.Nome != null && c.Nome.Contains(keyword)) ||
          (c.NomeComercial != null && c.NomeComercial.Contains(keyword)) ||
          (c.Abreviatura != null && c.Abreviatura.Contains(keyword))
        );
      }

      _ = Query.OrderBy(c => c.Nome).Take(10);
    }
  }
}

