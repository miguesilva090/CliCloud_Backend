using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Filters;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService
{
  public interface IMarcacaoConsultaService : ITransientService
  {
    Task<Response<IEnumerable<MarcacaoConsultaDTO>>> GetMarcacaoConsultaAsync(string keyword = "");
    Task<Response<IEnumerable<MarcacaoConsultaLightDTO>>> GetMarcacaoConsultaLightAsync(string keyword = "");
    Task<PaginatedResponse<MarcacaoConsultaTableDTO>> GetMarcacaoConsultaPaginatedAsync(MarcacaoConsultaTableFilter filter);
    Task<Response<IEnumerable<MarcacaoConsultaTableDTO>>> GetAllMarcacaoConsultaAsync(MarcacaoConsultaAllFilter? filter);
    Task<Response<MarcacaoConsultaDTO>> GetMarcacaoConsultaAsync(Guid id);
    Task<Response<Guid>> CreateMarcacaoConsultaAsync(CreateMarcacaoConsultaRequest request);
    Task<Response<Guid>> UpdateMarcacaoConsultaAsync(UpdateMarcacaoConsultaRequest request, Guid id);
    Task<Response<Guid>> DeleteMarcacaoConsultaAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleMarcacaoConsultaAsync(IEnumerable<Guid> ids);
  }
}

