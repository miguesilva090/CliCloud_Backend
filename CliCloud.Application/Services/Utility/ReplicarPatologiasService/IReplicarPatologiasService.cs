using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarPatologiasService.DTOs;

namespace CliCloud.Application.Services.Utility.ReplicarPatologiasService;

public interface IReplicarPatologiasService : ITransientService
{
    Task<Response<ReplicarPatologiasResponse>> ReplicarPatologiasAsync(ReplicarPatologiasRequest request);
}