using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.Filters;

namespace CliCloud.Application.Services.Utility.FreguesiaService
{
    public interface IFreguesiaService : ITransientService
    {
        Task<Response<IEnumerable<FreguesiaDTO>>> GetFreguesiaAsync(string keyword = "");
        Task<Response<IEnumerable<FreguesiaLightDTO>>> GetFreguesiaLightAsync(string keyword = "");
        Task<Response<IEnumerable<FreguesiaTableDTO>>> GetAllFreguesiaAsync(FreguesiaAllFilter filter);
        Task<PaginatedResponse<FreguesiaTableDTO>> GetFreguesiaPaginatedAsync(FreguesiaTableFilter filter);
        Task<Response<FreguesiaDTO>> GetFreguesiaAsync(Guid id);
        Task<Response<Guid>> CreateFreguesiaAsync(CreateFreguesiaRequest request);
        Task<Response<Guid>> UpdateFreguesiaAsync(UpdateFreguesiaRequest request, Guid id);  
        Task<Response<Guid>> DeleteFreguesiaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleFreguesiaAsync(IEnumerable<Guid> ids);
    }
}
