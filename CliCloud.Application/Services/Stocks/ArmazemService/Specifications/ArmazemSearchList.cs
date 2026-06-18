using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;

namespace CliCloud.Application.Services.Stocks.ArmazemService.Specifications;

public class ArmazemSearchList : Specification<ArmazemEntity>
{
    public ArmazemSearchList(string? keyword, Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x =>
                x.Nome.Contains(k) ||
                x.Codigo.ToString().Contains(k) ||
                (x.Localidade != null && x.Localidade.Contains(k)));
        }

        _ = Query.OrderBy(x => x.Codigo);
    }
}
