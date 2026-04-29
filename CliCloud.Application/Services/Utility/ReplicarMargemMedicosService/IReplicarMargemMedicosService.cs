using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.DTOs;

namespace CliCloud.Application.Services.Utility.ReplicarMargemMedicosService;

public interface IReplicarMargemMedicosService : ITransientService
{
    Task<Response<ReplicarMargemMedicosResponse>> ReplicarAsync(ReplicarMargemMedicosRequest request);
}
