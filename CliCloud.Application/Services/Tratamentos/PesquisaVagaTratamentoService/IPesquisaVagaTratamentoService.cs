using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService;

public interface IPesquisaVagaTratamentoService : ITransientService
{
    Task<Response<PesquisaVagaResponse>> PesquisarAsync(PesquisaVagaRequest request);
}