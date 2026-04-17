using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ClinicaService.DTOs;
using CliCloud.Application.Services.Core.ClinicaService.Filters;

namespace CliCloud.Application.Services.Core.ClinicaService
{
  public interface IClinicaService : ITransientService
  {
    Task<Response<IEnumerable<ClinicaDTO>>> GetClinicaAsync(string keyword = "");
    Task<Response<IEnumerable<ClinicaLightDTO>>> GetClinicaLightAsync(string keyword = "");
    Task<PaginatedResponse<ClinicaTableDTO>> GetClinicaPaginatedAsync(ClinicaTableFilter filter);
    Task<Response<IEnumerable<ClinicaTableDTO>>> GetAllClinicaAsync(ClinicaAllFilter filter);
    Task<Response<ClinicaDTO>> GetClinicaAsync(Guid id);
    Task<Response<Guid>> CreateClinicaAsync(CreateClinicaRequest request);
    Task<Response<Guid>> UpdateClinicaAsync(UpdateClinicaRequest request, Guid id);
    Task<Response<Guid>> DeleteClinicaAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleClinicaAsync(IEnumerable<Guid> ids);

    Task<Response<Guid>> SetDefaultClinicaAsync(Guid id, bool porDefeito);

    Task<Response<AvisosClinicaLegacyDTO>> GetAvisosClinicaAsync(Guid id);

    Task<Response<int[]>> GetFolgasClinicaAsync(Guid id);
    Task<Response<int?>> GetPortaCartaoClinicaAsync(Guid id);

    Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasAutocompleteAsync(string? q);

    Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasSelectedAutocompleteAsync(string? q, Guid currentClinicaId);

    Task<Response<int>> GetConfiguracaoAnoAtivaAsync(Guid clinicaId);
  }
}
