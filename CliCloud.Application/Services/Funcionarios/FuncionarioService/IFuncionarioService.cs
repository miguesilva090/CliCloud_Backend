using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.DTOs;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.Filters;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService
{
    public interface IFuncionarioService : ITransientService
    {
        Task<Response<IEnumerable<FuncionarioDTO>>> GetFuncionarioAsync(string keyword = "");
        Task<Response<IEnumerable<FuncionarioLightDTO>>> GetFuncionarioLightAsync(string keyword = "");
        Task<PaginatedResponse<FuncionarioTableDTO>> GetFuncionarioPaginatedAsync(FuncionarioTableFilter filter);
        Task<Response<IEnumerable<FuncionarioTableDTO>>> GetAllFuncionarioAsync(FuncionarioAllFilter filter);
        Task<Response<FuncionarioDTO>> GetFuncionarioAsync(Guid id);
        Task<Response<FuncionarioDTO>> GetFuncionarioByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<FuncionarioDTO>>> GetFuncionarioByNameAsync(string nome);
        Task<Response<Guid>> CreateFuncionarioAsync(CreateFuncionarioRequest request);
        Task<Response<Guid>> UpdateFuncionarioAsync(UpdateFuncionarioRequest request, Guid id);
        Task<Response<Guid>> DeleteFuncionarioAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleFuncionarioAsync(IEnumerable<Guid> ids);
    }
}
