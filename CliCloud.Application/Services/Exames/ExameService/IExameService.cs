using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Exames.ExameService.DTOs;
using CliCloud.Application.Services.Exames.ExameService.Filters;

namespace CliCloud.Application.Services.Exames.ExameService
{
  public interface IExameService : ITransientService
  {
    Task<Response<IEnumerable<ExameDTO>>> GetExameAsync(string keyword = "");
    Task<Response<IEnumerable<ExameLightDTO>>> GetExameLightAsync(string keyword = "");
    Task<PaginatedResponse<ExameTableDTO>> GetExamePaginatedAsync(ExameTableFilter filter);
    Task<Response<IEnumerable<ExameTableDTO>>> GetAllExameAsync(ExameAllFilter? filter);
    Task<Response<ExameDTO>> GetExameAsync(Guid id);
    Task<Response<IEnumerable<ResultadoExameTableDTO>>> GetResultadosByExameAsync(Guid exameId);
    Task<Response<Guid>> UpsertResultadoLinhaAsync(Guid exameId, Guid linhaId, string? valor, string? referencia, string? obs);
    Task<Response<Guid>> CreateExameAsync(CreateExameRequest request);
    Task<Response<Guid>> UpdateExameAsync(UpdateExameRequest request, Guid id);
    Task<Response<Guid>> DeleteExameAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleExameAsync(IEnumerable<Guid> ids);
    Task<Response<ExamePrescricaoReportDTO>> GetExameReportAsync(Guid exameId);
  }
}
