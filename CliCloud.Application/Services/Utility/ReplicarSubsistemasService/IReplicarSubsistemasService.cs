using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarSubsistemasService.DTOs;

namespace CliCloud.Application.Services.Utility.ReplicarSubsistemasService;

public interface IReplicarSubsistemasService : ITransientService
{
    Task<Response<ReplicarSubsistemasResponse>> ReplicarSubsistemasAsync(ReplicarSubsistemasRequest request);
}
