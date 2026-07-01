using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.Specifications;

public class WebserviceAdsePorClinicaSpec : Specification<WebserviceAdse>
{
    public WebserviceAdsePorClinicaSpec(Guid clinicaId) => 
        Query.Where(x => x.ClinicaId == clinicaId && x.DeletedOn == null);
}