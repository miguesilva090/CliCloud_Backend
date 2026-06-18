using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Services.Stocks.ArmazemService.Filters;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;

namespace CliCloud.Application.Services.Stocks.ArmazemService.Specifications;

public class ArmazemSearchTable : Specification<ArmazemEntity>
{
    public ArmazemSearchTable(ArmazemTableFilter filter, Guid clinicaId, string? dynamicOrder = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(filter.FiltroBox))
        {
            string fb = filter.FiltroBox.Trim();
            _ = Query.Where(x =>
                x.Nome.Contains(fb) ||
                x.Codigo.ToString().Contains(fb) ||
                (x.Localidade != null && x.Localidade.Contains(fb)));
        }

        if (filter.CodigoDe.HasValue)
            _ = Query.Where(x => x.Codigo >= filter.CodigoDe.Value);
        if (filter.CodigoAte.HasValue)
            _ = Query.Where(x => x.Codigo <= filter.CodigoAte.Value);
        if (!string.IsNullOrWhiteSpace(filter.NomeDe))
            _ = Query.Where(x => string.Compare(x.Nome, filter.NomeDe.Trim()) >= 0);
        if (!string.IsNullOrWhiteSpace(filter.NomeAte))
            _ = Query.Where(x => string.Compare(x.Nome, filter.NomeAte.Trim()) <= 0);
        if (filter.ArmazemGeral.HasValue)
            _ = Query.Where(x => x.ArmazemGeral == filter.ArmazemGeral.Value);

        if (filter.Filters != null)
        {
            foreach (var f in filter.Filters)
            {
                switch ((f.Id ?? "").ToLowerInvariant())
                {
                    case "codigo":
                        if (int.TryParse(f.Value, out int codigo))
                            _ = Query.Where(x => x.Codigo == codigo);
                        break;
                    case "nome":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Nome.Contains(f.Value));
                        break;
                    case "localidade":
                        if (!string.IsNullOrWhiteSpace(f.Value))
                            _ = Query.Where(x => x.Localidade != null && x.Localidade.Contains(f.Value));
                        break;
                    case "armazemgeral":
                        if (bool.TryParse(f.Value, out bool armazemGeral))
                            _ = Query.Where(x => x.ArmazemGeral == armazemGeral);
                        break;
                }
            }
        }

        _ = string.IsNullOrEmpty(dynamicOrder)
            ? Query.OrderBy(x => x.Codigo)
            : Query.OrderBy(dynamicOrder);
    }
}
