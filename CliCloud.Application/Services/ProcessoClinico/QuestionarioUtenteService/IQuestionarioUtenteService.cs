using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService
{
  public interface IQuestionarioUtenteService : ITransientService
  {
    Task<Response<QuestionarioUtenteDTO>> GetQuestionarioAsync(Guid id);

    Task<PaginatedResponse<QuestionarioUtenteTableDTO>> GetQuestionariosPaginatedAsync(
      QuestionarioUtenteTableFilter filter
    );

    Task<Response<Guid>> CreateQuestionarioAsync(CreateQuestionarioUtenteRequest request);

    Task<Response<Guid>> UpdateQuestionarioAsync(UpdateQuestionarioUtenteRequest request, Guid id);

    Task<Response<Guid>> DeleteQuestionarioAsync(Guid id);
  }
}

