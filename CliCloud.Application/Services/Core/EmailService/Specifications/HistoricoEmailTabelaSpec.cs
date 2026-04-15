using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Core.Email;

namespace CliCloud.Application.Services.Core.EmailService.Specifications;

public class HistoricoEmailTabelaSpec : Specification<HistoricoEmail>
{
    public HistoricoEmailTabelaSpec(Guid clinicaId, List<TableFilter> filters, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (filters != null && filters.Count > 0)
        {
            foreach (var f in filters)
            {
                switch (f.Id.ToLowerInvariant())
                {
                    case "status":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Status.Contains(f.Value));
                        break;
                    case "modulo":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Modulo.Contains(f.Value));
                        break;
                    case "emaildestino":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => (x.EmailDestino ?? string.Empty).Contains(f.Value));
                        break;
                    case "nomeutente":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => (x.NomeUtente ?? string.Empty).Contains(f.Value));
                        break;
                }
            }
        }

        if (string.IsNullOrWhiteSpace(dynamicOrder))
            _ = Query.OrderByDescending(x => x.DataHoraCriacao);
        else
            _ = Query.OrderBy(dynamicOrder);
    }
}
