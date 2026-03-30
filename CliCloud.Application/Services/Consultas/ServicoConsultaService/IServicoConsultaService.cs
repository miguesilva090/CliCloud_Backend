using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.Filters;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService
{
  public interface IServicoConsultaService : ITransientService
  {
    Task<Response<IEnumerable<ServicoConsultaDTO>>> GetServicoConsultaAsync(string keyword = "");
    Task<Response<IEnumerable<ServicoConsultaLightDTO>>> GetServicoConsultaLightAsync(string keyword = "");
    Task<PaginatedResponse<ServicoConsultaTableDTO>> GetServicoConsultaPaginatedAsync(ServicoConsultaTableFilter filter);
    Task<Response<IEnumerable<ServicoConsultaTableDTO>>> GetAllServicoConsultaAsync(ServicoConsultaAllFilter? filter);
    Task<Response<ServicoConsultaDTO>> GetServicoConsultaAsync(Guid id);
    Task<Response<Guid>> CreateServicoConsultaAsync(CreateServicoConsultaRequest request);
    Task<Response<Guid>> UpdateServicoConsultaAsync(UpdateServicoConsultaRequest request, Guid id);
    Task<Response<Guid>> DeleteServicoConsultaAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoConsultaAsync(IEnumerable<Guid> ids);
  }
}

