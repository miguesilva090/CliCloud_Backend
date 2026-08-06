using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService;

public interface IMarcacaoAutomaticaTratamentoService : ITransientService
{
  Task<Response<MarcacaoAutomaticaPreviewResponse>> PreviewAsync(
    MarcacaoAutomaticaPreviewRequest request
  );

  Task<Response<Guid>> ConfirmAsync(MarcacaoAutomaticaConfirmRequest request);
}