using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;

namespace CliCloud.Application.Services.Stocks.ArmazemService.Specifications;

public class ArmazemByClinicaSpec : Specification<ArmazemEntity>
{
    public ArmazemByClinicaSpec(Guid clinicaId)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}
