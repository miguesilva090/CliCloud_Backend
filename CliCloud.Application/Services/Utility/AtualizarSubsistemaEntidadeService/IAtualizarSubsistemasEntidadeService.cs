using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService.DTOs;

namespace CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService;

public interface IAtualizarSubsistemasEntidadeService : ITransientService
{
    Task<Response<AtualizarSubsistemasEntidadeResponse>> AtualizarAsync(AtualizarSubsistemasEntidadeRequest request);
}