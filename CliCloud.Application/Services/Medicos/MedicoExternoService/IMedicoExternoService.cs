using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.MedicoExternoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoExternoService.Filters;

namespace CliCloud.Application.Services.Medicos.MedicoExternoService
{
    public interface IMedicoExternoService : ITransientService
    {
        Task<Response<IEnumerable<MedicoExternoDTO>>> GetMedicoExternoAsync(string keyword = "");
        Task<Response<IEnumerable<MedicoExternoLightDTO>>> GetMedicoExternoLightAsync(string keyword = "");
        Task<PaginatedResponse<MedicoExternoTableDTO>> GetMedicoExternoPaginatedAsync(MedicoExternoTableFilter filter);
        Task<Response<IEnumerable<MedicoExternoTableDTO>>> GetAllMedicoExternoAsync(MedicoExternoAllFilter filter);
        Task<Response<MedicoExternoDTO>> GetMedicoExternoAsync(Guid id);
        Task<Response<MedicoExternoDTO>> GetMedicoExternoByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<MedicoExternoDTO>>> GetMedicoExternoByNameAsync(string nome);
        Task<Response<Guid>> CreateMedicoExternoAsync(CreateMedicoExternoRequest request);
        Task<Response<Guid>> UpdateMedicoExternoAsync(UpdateMedicoExternoRequest request, Guid id);
        Task<Response<Guid>> DeleteMedicoExternoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMedicoExternoAsync(IEnumerable<Guid> ids);
    }
}
