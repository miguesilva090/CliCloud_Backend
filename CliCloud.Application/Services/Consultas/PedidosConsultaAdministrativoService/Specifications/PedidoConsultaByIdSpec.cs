using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class PedidoConsultaByIdSpec : Specification<PedidoConsulta>
{
  public PedidoConsultaByIdSpec(int codigo, Guid? clinicaId = null)
  {
    _ = Query.Where(x => x.Id == codigo);
    if (clinicaId.HasValue)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId.Value);
    }

    _ = Query.Include(x => x.UtentePedido);
  }
}
