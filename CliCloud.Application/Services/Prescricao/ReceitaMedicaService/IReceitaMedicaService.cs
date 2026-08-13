using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Filters;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService
{
  public interface IReceitaMedicaService : ITransientService
  {
    Task<PaginatedResponse<ReceitaMedicaTableDTO>> GetPaginatedAsync(ReceitaMedicaTableFilter filter);
    Task<Response<ReceitaMedicaDTO>> GetByIdAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateReceitaMedicaRequest request);
    Task<Response<Guid>> UpdateAsync(Guid id, UpdateReceitaMedicaRequest request);
    /// <summary>Paridade PrescricaoRSPSend / EnviarDesmaterializadas (requer token prescritor).</summary>
    Task<Response<Guid>> EnviarAsync(Guid id, Guid clinicaId, EnviarReceitaMedicaRequest request);
    /// <summary>Paridade PrescricaoRSPDel: local se não enviada; SPMS se já enviada.</summary>
    Task<Response<Guid>> AnularAsync(Guid id, AnularReceitaMedicaRequest request, Guid clinicaId);
  }
}
