using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Filters;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class PedidoConsultaSearchTable : Specification<PedidoConsulta>
{
  public PedidoConsultaSearchTable(PedidoConsultaTableFilter filter, string? dynamicOrder = "")
  {
    _ = Query.Include(x => x.UtentePedido);

    if (filter.ClinicaId.HasValue)
    {
      Guid clinicaId = filter.ClinicaId.Value;
      _ = Query.Where(x => x.ClinicaId == clinicaId);
    }

    if (filter.AgendadoSim == true && filter.AgendadoNao != true)
    {
      _ = Query.Where(x => x.Agendado);
    }
    else if (filter.AgendadoNao == true && filter.AgendadoSim != true)
    {
      _ = Query.Where(x => !x.Agendado);
    }

    if (filter.RecusadoSim == true && filter.RecusadoNao != true)
    {
      _ = Query.Where(x => x.Recusado);
    }
    else if (filter.RecusadoNao == true && filter.RecusadoSim != true)
    {
      _ = Query.Where(x => !x.Recusado);
    }

    if (filter.EmailPedidoSim == true && filter.EmailPedidoNao != true)
    {
      _ = Query.Where(x => x.EmailPedido);
    }
    else if (filter.EmailPedidoNao == true && filter.EmailPedidoSim != true)
    {
      _ = Query.Where(x => !x.EmailPedido);
    }

    if (filter.SmsPedidoSim == true && filter.SmsPedidoNao != true)
    {
      _ = Query.Where(x => x.SmsPedido);
    }
    else if (filter.SmsPedidoNao == true && filter.SmsPedidoSim != true)
    {
      _ = Query.Where(x => !x.SmsPedido);
    }

    if (filter.EmailAgendadoSim == true && filter.EmailAgendadoNao != true)
    {
      _ = Query.Where(x => x.EmailAgendado);
    }
    else if (filter.EmailAgendadoNao == true && filter.EmailAgendadoSim != true)
    {
      _ = Query.Where(x => !x.EmailAgendado);
    }

    if (filter.SmsAgendadoSim == true && filter.SmsAgendadoNao != true)
    {
      _ = Query.Where(x => x.SmsAgendado);
    }
    else if (filter.SmsAgendadoNao == true && filter.SmsAgendadoSim != true)
    {
      _ = Query.Where(x => !x.SmsAgendado);
    }

    if (filter.DataDe.HasValue)
    {
      DateTime d = filter.DataDe.Value.Date;
      _ = Query.Where(x => x.Data.Date >= d);
    }

    if (filter.DataAte.HasValue)
    {
      DateTime d = filter.DataAte.Value.Date;
      _ = Query.Where(x => x.Data.Date <= d);
    }

    if (!string.IsNullOrWhiteSpace(filter.CodigoMedicoDe))
    {
      string de = filter.CodigoMedicoDe.Trim();
      _ = Query.Where(x => x.CodigoMedico != null && string.Compare(x.CodigoMedico, de) >= 0);
    }

    if (!string.IsNullOrWhiteSpace(filter.CodigoMedicoAte))
    {
      string ate = filter.CodigoMedicoAte.Trim();
      _ = Query.Where(x => x.CodigoMedico != null && string.Compare(x.CodigoMedico, ate) <= 0);
    }

    foreach (TableFilter f in filter.Filters ?? [])
    {
      string id = (f.Id ?? string.Empty).ToLowerInvariant();
      string? val = f.Value;
      if (string.IsNullOrWhiteSpace(val))
      {
        continue;
      }

      switch (id)
      {
        case "filtrobox":
        case "nome":
          _ = Query.Where(x =>
            x.UtentePedido != null
            && (
              (x.UtentePedido.Nome != null && x.UtentePedido.Nome.Contains(val))
              || (x.UtentePedido.Email != null && x.UtentePedido.Email.Contains(val))
              || (x.UtentePedido.Telemovel != null && x.UtentePedido.Telemovel.Contains(val))
            )
          );
          break;
        case "codigo":
          if (int.TryParse(val, out int codigo))
          {
            _ = Query.Where(x => x.Id == codigo);
          }
          break;
      }
    }

    if (!string.IsNullOrWhiteSpace(dynamicOrder))
    {
      _ = Query.OrderBy(dynamicOrder);
    }
    else
    {
      _ = Query.OrderBy(x => x.Id);
    }
  }

}
