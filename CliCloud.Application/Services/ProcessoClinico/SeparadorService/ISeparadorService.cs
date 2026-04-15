using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService;

public interface ISeparadorService : ITransientService
{
    Task<Response<IEnumerable<SeparadorDTO>>> GetSeparadorAsync(string keyword = "");
    Task<Response<IEnumerable<SeparadorFichaClinicaDTO>>> GetSeparadoresFichaClinicaVisiveisAsync(
        Guid clinicaId,
        Guid? medicoId,
        Guid? especialidadeId
    );
    Task<PaginatedResponse<SeparadorDTO>> GetSeparadorPaginatedAsync(SeparadorTableFilter filter);
    Task<Response<SeparadorDTO>> GetSeparadorAsync(Guid id);
    Task<Response<Guid>> CreateSeparadorAsync(CreateSeparadorRequest request);
    Task<Response<Guid>> UpdateSeparadorAsync(UpdateSeparadorRequest request, Guid id);
    Task<Response<Guid>> DeleteSeparadorAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleSeparadorAsync(IEnumerable<Guid> ids);
}
