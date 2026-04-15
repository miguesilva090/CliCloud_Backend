using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigCartaConducaoService.DTOs;

namespace CliCloud.Application.Services.Core.ConfigCartaConducaoService
{
    public interface IConfigCartaConducaoService : ITransientService
    {
        Task<Response<ConfigCartaConducaoDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
        Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigCartaConducaoRequest request);
    }
}