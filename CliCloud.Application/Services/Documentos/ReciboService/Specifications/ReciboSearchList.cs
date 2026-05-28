using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.ReciboService.Specifications
{
  public class ReciboSearchList : Specification<Recibo>
  {
    public ReciboSearchList(string? keyword = "", Guid? clinicaId = null)
    {
      if (clinicaId.HasValue)
      {
        _ = Query.Where(x => x.ClinicaId == clinicaId.Value);
      }

      if (!string.IsNullOrWhiteSpace(keyword) && int.TryParse(keyword, out var num))
        _ = Query.Where(x => x.NumeroDocumento == num);
      _ = Query.OrderByDescending(x => x.Data).ThenByDescending(x => x.NumeroDocumento);
    }
  }
}
