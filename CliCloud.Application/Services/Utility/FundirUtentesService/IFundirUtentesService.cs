using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.FundirUtentesService.DTOs;

namespace CliCloud.Application.Services.Utility.FundirUtentesService;

public interface IFundirUtentesService : ITransientService
{
    Task<Response<FundirUtentesResponse>> FundirUtentesAsync(
        FundirUtentesRequest request,
        CancellationToken cancellationToken = default);
}
