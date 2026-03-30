using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService
{
    public interface IFichaClinicaSecaoTemplateService : ITransientService
    {
        Task<Response<IEnumerable<FichaClinicaSecaoTemplateDTO>>> GetFichaClinicaSecaoTemplateAsync(string keyword = "");
        Task<PaginatedResponse<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplatePaginatedAsync(FichaClinicaSecaoTemplateTableFilter filter);
        Task<Response<FichaClinicaSecaoTemplateDTO>> GetFichaClinicaSecaoTemplateAsync(Guid id);
        Task<Response<Guid>> CreateFichaClinicaSecaoTemplateAsync(CreateFichaClinicaSecaoTemplateRequest request);
        Task<Response<Guid>> UpdateFichaClinicaSecaoTemplateAsync(UpdateFichaClinicaSecaoTemplateRequest request, Guid id);
        Task<Response<Guid>> DeleteFichaClinicaSecaoTemplateAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoTemplateAsync(IEnumerable<Guid> ids);
    }
}
