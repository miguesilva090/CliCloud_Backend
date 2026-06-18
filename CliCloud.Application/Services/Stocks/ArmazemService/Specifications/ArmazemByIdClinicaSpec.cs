using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;

namespace CliCloud.Application.Services.Stocks.ArmazemService.Specifications;

public class ArmazemByIdClinicaSpec : Specification<ArmazemEntity>
{
    public ArmazemByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId)
            .Include(x => x.CodigoPostal);
    }
}
