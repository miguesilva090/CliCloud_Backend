using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.RuaService.Specifications
{
  public class RuaMatchNameAndLocation : Specification<Rua>
  {
    public RuaMatchNameAndLocation(string name, Guid freguesiaId, Guid codigoPostalId)
    {
      if (!string.IsNullOrWhiteSpace(name))
      {
        var nameNorm = (name ?? "").Trim().ToLowerInvariant();
        _ = Query.Where(h =>
          h.Nome != null &&
          h.Nome.ToLower() == nameNorm &&
          h.FreguesiaId == freguesiaId &&
          h.CodigoPostalId == codigoPostalId);
      }
      _ = Query.OrderBy(h => h.Nome);
    }
  }
}