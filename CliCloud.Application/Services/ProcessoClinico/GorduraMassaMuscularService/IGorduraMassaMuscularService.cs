using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService
{
    public interface IGorduraMassaMuscularService : ITransientService
    {
        Task<Response<IEnumerable<GorduraMassaMuscularDTO>>> GetGorduraMassaMuscularAsync(string keyword = "");
        Task<Response<IEnumerable<GorduraMassaMuscularLightDTO>>> GetGorduraMassaMuscularLightAsync(string keyword = "");
        Task<PaginatedResponse<GorduraMassaMuscularDTO>> GetGorduraMassaMuscularPaginatedAsync(GorduraMassaMuscularTableFilter filter);
        Task<Response<IEnumerable<GorduraMassaMuscularTableDTO>>> GetAllGorduraMassaMuscularAsync(GorduraMassaMuscularAllFilter filter);
        Task<Response<GorduraMassaMuscularDTO>> GetGorduraMassaMuscularAsync(Guid id);
        Task<Response<Guid>> CreateGorduraMassaMuscularAsync(CreateGorduraMassaMuscularRequest request);
        Task<Response<Guid>> UpdateGorduraMassaMuscularAsync(UpdateGorduraMassaMuscularRequest request, Guid id);
        Task<Response<Guid>> DeleteGorduraMassaMuscularAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGorduraMassaMuscularAsync(IEnumerable<Guid> ids);
    }
}
