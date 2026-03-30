using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using CliCloud.Application.Services.Empresas.EmpresaService.Filters;

namespace CliCloud.Application.Services.Empresas.EmpresaService
{
    public interface IEmpresaService : ITransientService
    {
        Task<Response<IEnumerable<EmpresaDTO>>> GetEmpresaAsync(string keyword = "");
        Task<PaginatedResponse<EmpresaTableDTO>> GetEmpresaPaginatedAsync(EmpresaTableFilter filter);
        Task<Response<IEnumerable<EmpresaTableDTO>>> GetAllEmpresaAsync(EmpresaAllFilter filter);
        Task<Response<EmpresaDTO>> GetEmpresaAsync(Guid id);
        Task<Response<EmpresaDTO>> GetEmpresaByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<EmpresaDTO>>> GetEmpresaByNameAsync(string nome);
        Task<Response<Guid>> CreateEmpresaAsync(CreateEmpresaRequest request);
        Task<Response<Guid>> UpdateEmpresaAsync(UpdateEmpresaRequest request, Guid id);
        Task<Response<Guid>> DeleteEmpresaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEmpresaAsync(IEnumerable<Guid> ids);
    }
}

