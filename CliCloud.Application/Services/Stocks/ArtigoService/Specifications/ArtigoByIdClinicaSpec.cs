using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;

namespace CliCloud.Application.Services.Stocks.ArtigoService.Specifications;

public class ArtigoByIdClinicaSpec : Specification<ArtigoEntity>
{
    public ArtigoByIdClinicaSpec(Guid id, Guid clinicaId)
    {
        _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId)
            .Include(x => x.UnidadeMedida)
            .Include(x => x.FamiliaArtigo)
            .Include(x => x.TaxaIva)
            .Include(x => x.MotivoIsencao)
            .Include(x => x.Armazem);
    }
}