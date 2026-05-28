using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.ReciboService.Specifications
{
  public class ReciboByIdClinicaSpec : Specification<Recibo>
  {
    public ReciboByIdClinicaSpec(Guid id, Guid clinicaId)
    {
      _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
    }
  }
}
