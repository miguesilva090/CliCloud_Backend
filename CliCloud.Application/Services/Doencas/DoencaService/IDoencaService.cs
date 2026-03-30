using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;
using CliCloud.Application.Services.Doencas.DoencaService.Filters;

namespace CliCloud.Application.Services.Doencas.DoencaService
{
    public interface IDoencaService : ITransientService
    {
        Task<Response<IEnumerable<DoencaDTO>>> GetDoencasAsync(string keyword = "");
        Task<PaginatedResponse<DoencaDTO>> GetDoencasPaginatedAsync(DoencaTableFilter filter);
        Task<Response<DoencaDTO>> GetDoencaAsync(Guid id);
        Task<Response<Guid>> UpdateDoencaAsync(UpdateDoencaRequest request, Guid id);
        Task<Response<Guid>> DeleteDoencaAsync(Guid id);
    }
}
