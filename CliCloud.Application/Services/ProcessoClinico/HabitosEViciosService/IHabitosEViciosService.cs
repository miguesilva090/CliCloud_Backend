using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService
{
    public interface IHabitosEViciosService : ITransientService
    {
        Task<Response<IEnumerable<HabitosEViciosDTO>>> GetHabitosEViciosAsync(string keyword = "");
        Task<Response<IEnumerable<HabitosEViciosLightDTO>>> GetHabitosEViciosLightAsync(string keyword = "");
        Task<PaginatedResponse<HabitosEViciosTableDTO>> GetHabitosEViciosPaginatedAsync(HabitosEViciosTableFilter filter);
        Task<Response<IEnumerable<HabitosEViciosTableDTO>>> GetAllHabitosEViciosAsync(HabitosEViciosAllFilter filter);
        Task<Response<HabitosEViciosDTO>> GetHabitosEViciosAsync(Guid id);
        Task<Response<Guid>> CreateHabitosEViciosAsync(CreateHabitosEViciosRequest request);
        Task<Response<Guid>> UpdateHabitosEViciosAsync(UpdateHabitosEViciosRequest request, Guid id);
        Task<Response<Guid>> DeleteHabitosEViciosAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHabitosEViciosAsync(IEnumerable<Guid> ids);
    }
}
