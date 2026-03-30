using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.Filters;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService
{
    public interface ITaxaIvaService : ITransientService
    {
        Task<Response<IEnumerable<TaxaIvaDTO>>> GetTaxaIvaAsync(string keyword = "");
        Task<Response<IEnumerable<TaxaIvaLightDTO>>> GetTaxaIvaLightAsync(string keyword = "");
        Task<PaginatedResponse<TaxaIvaTableDTO>> GetTaxaIvaPaginatedAsync(TaxaIvaTableFilter filter);
        Task<Response<IEnumerable<TaxaIvaTableDTO>>> GetAllTaxaIvaAsync(TaxaIvaAllFilter filter);
        Task<Response<TaxaIvaDTO>> GetTaxaIvaAsync(Guid id);
        Task<Response<Guid>> CreateTaxaIvaAsync(CreateTaxaIvaRequest request);
        Task<Response<Guid>> UpdateTaxaIvaAsync(UpdateTaxaIvaRequest request, Guid id);
        Task<Response<Guid>> DeleteTaxaIvaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTaxaIvaAsync(IEnumerable<Guid> ids);
    }
}
