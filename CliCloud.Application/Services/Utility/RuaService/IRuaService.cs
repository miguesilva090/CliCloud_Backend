using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.Filters;

namespace CliCloud.Application.Services.Utility.RuaService
{
    public interface IRuaService : ITransientService
    {
        Task<Response<IEnumerable<RuaDTO>>> GetRuaAsync(string keyword = "");
        Task<Response<IEnumerable<RuaLightDTO>>> GetRuaLightAsync(string keyword = "");
        Task<PaginatedResponse<RuaTableDTO>> GetRuaPaginatedAsync(RuaTableFilter filter);
        Task<Response<IEnumerable<RuaTableDTO>>> GetAllRuaAsync(RuaAllFilter filter);
        Task<Response<RuaDTO>> GetRuaAsync(Guid id);
        Task<Response<Guid>> CreateRuaAsync(CreateRuaRequest request);
        Task<Response<Guid>> UpdateRuaAsync(UpdateRuaRequest request, Guid id);  
        Task<Response<Guid>> DeleteRuaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleRuasAsync(IEnumerable<Guid> ids);
    }
}
