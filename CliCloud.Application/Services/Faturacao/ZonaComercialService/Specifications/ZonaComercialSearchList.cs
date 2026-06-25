using Ardalis.Specification;
using ZonaComercialEntity = CliCloud.Domain.Entities.Faturacao.ZonaComercial;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.Specifications;

public class ZonaComercialSearchList : Specification<ZonaComercialEntity>
{
    public ZonaComercialSearchList(Guid clinicaId, string? keyword)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);

        if(!string.IsNullOrWhiteSpace(keyword))
        {
            string k = keyword.Trim();
            _ = Query.Where(x =>
                x.Descricao.Contains(k) ||
                x.Codigo.ToString().Contains(k));
        }

        _ = Query.OrderBy(x => x.Codigo);
    }
}